using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenerativeAI;
using JobBoard.Domain.DTO.AIEmbeddingDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Repositories.Persistence;
using JobBoard.Services.AIServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JobBoard.Services.AIEmbeddingService
{
    public class AIEmbeddingService : IAIEmbeddingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmbeddingModel _embeddingModel;
        private readonly IMapper _mapper;
        private readonly IGeminiChatService _chatService;

        public AIEmbeddingService(IUnitOfWork unitOfWork, IGeminiChatService chatService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _chatService = chatService;
            var apiKey = chatService.GetApiKey();
            _embeddingModel = new EmbeddingModel(apiKey, "text-embedding-004");

        }
        public async Task GenerateEmbeddingsForJobsAsync()
        {
            var jobRepo = _unitOfWork.Repository<Job>();
            var jobs = await jobRepo.GetAllAsync();

            var embeddingRepo = _unitOfWork.Repository<AIEmbedding>();
            var existingEmbeddings = await embeddingRepo.GetAllAsync();

            var existingJobIds = existingEmbeddings
            .Where(e => e.EntityType == "Job")
            .Select(e => e.EntityId)
            .ToHashSet();

            foreach (var job in jobs.OfType<Job>()) {

                if (existingJobIds.Contains(job.Id)) continue; // skip if already embedded


                var dto = _mapper.Map<JobDto>(job);
                var content = BuildJobContent(dto);

                var response = await _embeddingModel.EmbedContentAsync(content);
                if (response == null || response.Embedding == null) continue;

                //convert to float[]
                if (response?.Embedding?.Values == null || !response.Embedding.Values.Any())
                    continue;
                var vector = response.Embedding.Values?.Select(v => (float)v).ToArray(); 


                //save content in AIEmbedding Entity
                var embedding = new AIEmbedding
                {
                    EntityType = "Job",
                    EntityId = job.Id,
                    Content = content,
                    EmbeddingVector = vector,
                    CreatedAt = DateTime.UtcNow
                };

                await embeddingRepo.AddAsync(embedding);

            }

            await _unitOfWork.CompleteAsync();
        }


        private string BuildJobContent(JobDto job)
        {
            return $"""
            Job Title: {job.Title}
            Description: {job.Description}
            Requirements: {job.Requirements}
            Education Level: {job.EducationLevel}
            Experience Level: {job.ExperienceLevel}
            Skills: {string.Join(", ", job.Skills)}
            Categories: {string.Join(", ", job.Categories)}
            Salary: {job.Salary?.ToString() ?? "Not specified"}
            Company: {job.CompanyName}
            Location: {job.CompanyLocation}
            Website: {job.Website}
            """;
        }

        /*Find the functions that are closest in meaning to a given word or phrase using embedding.*/

        /* We use the query typed by the user, generate an embedding vector for it,
         * calculate the cosine similarity between it and all the embeddings stored in the database,
         * and then return the top K results as functions that are similar in meaning.*/
        public async Task<List<SemanticSearchResultDto>> SearchJobsByMeaningAsync(string query, int topK)
        {
            // Step 1: Generate embedding for the query

            var queryResponse = await _embeddingModel.EmbedContentAsync(query);
            if (queryResponse?.Embedding?.Values == null || !queryResponse.Embedding.Values.Any())
                return new List<SemanticSearchResultDto>();

            var queryVector = queryResponse.Embedding.Values.Select(v => (float)v).ToArray();

            // Step 2: Get all job embeddings from the DB

            var embeddingRepo = _unitOfWork.Repository<AIEmbedding>();
            var allEmbeddings = await embeddingRepo.GetAllAsync();
            var jobEmbeddings = allEmbeddings.Where(e => e.EntityType == "Job" && e.EmbeddingVector != null).ToList();

            // Step 3: Compute similarity between query and each job

            var similarities = new List<(AIEmbedding Embedding, float Score)>();

            foreach (var jobEmbedding in jobEmbeddings)
            {
                var score = CosineSimilarity(queryVector, jobEmbedding.EmbeddingVector);
                similarities.Add((jobEmbedding, score));
            }


            // Step 4: Order by similarity and return top K

            var topResults = similarities
            .OrderByDescending(x => x.Score)
            .Take(topK)
            .ToList();

            // Load the actual job data and prepare result DTOs

            var jobRepo = _unitOfWork.Repository<Job>();
            var resultDtos = new List<SemanticSearchResultDto>();

            foreach (var (embedding, score) in topResults)
            {
                var job = await jobRepo.GetByIdAsync(embedding.EntityId);
                if (job == null) continue;

                var dto = _mapper.Map<JobDto>(job);

                resultDtos.Add(new SemanticSearchResultDto
                {
                    Job = dto,
                    Similarity = score
                });
            }

            return resultDtos;

        }

        private float CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            float dotProduct = 0;
            float magnitudeA = 0;
            float magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += vectorA[i] * vectorA[i];
                magnitudeB += vectorB[i] * vectorB[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;

            return dotProduct / (float)(Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
        }

        public async Task<string> GetJobAnswerFromGeminiAsync(string userQuestion)
        {
            //use semantic search to pull similar jobs (most top 3 , TopK=3)
            var topJobs = await SearchJobsByMeaningAsync(userQuestion, 3);

            if (topJobs == null || !topJobs.Any())
                return "Sorry, I couldn't find any jobs matching your query.";

            var contextBuilder = new StringBuilder();

            foreach (var result in topJobs)
            {
                var job = result.Job;
                contextBuilder.AppendLine($"""
                Job Title: {job.Title}
                Description: {job.Description}
                Requirements: {job.Requirements}
                Skills: {string.Join(", ", job.Skills)}
                Location: {job.CompanyLocation}
                """);
            }


            //prepare the prompt
            var finalPrompt = $"""
                    You are a job assistant. Based on the following job listings, answer the user's question:

                    {contextBuilder}

                    User's question: {userQuestion}
                    """;

            var response = await _chatService.AskGeminiAsync(finalPrompt);

            return response ?? "Error throgh Generation";
        }

        public async Task GenerateEmbeddingForJobAsync(Job job)
        {
            var embeddingRepo = _unitOfWork.Repository<AIEmbedding>();

            // skip if already exists
            var exists = await embeddingRepo.FindAsync(e =>
                e.EntityType == "Job" && e.EntityId == job.Id);
            if (exists != null) return;

            var dto = _mapper.Map<JobDto>(job);
            var content = BuildJobContent(dto);

            var response = await _embeddingModel.EmbedContentAsync(content);
            if (response?.Embedding?.Values == null || !response.Embedding.Values.Any())
                return;

            var vector = response.Embedding.Values.Select(v => (float)v).ToArray();

            var embedding = new AIEmbedding
            {
                EntityType = "Job",
                EntityId = job.Id,
                Content = content,
                EmbeddingVector = vector,
                CreatedAt = DateTime.UtcNow
            };

            await embeddingRepo.AddAsync(embedding);
            await _unitOfWork.CompleteAsync();
        }

    }
}
