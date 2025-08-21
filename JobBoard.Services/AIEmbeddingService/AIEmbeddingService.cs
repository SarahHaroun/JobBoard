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
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Persistence;
using JobBoard.Repositories.Specifications;
using JobBoard.Services.AIChatHistoryServices;
using JobBoard.Services.AIServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace JobBoard.Services.AIEmbeddingService
{
    public class AIEmbeddingService : IAIEmbeddingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmbeddingModel _embeddingModel;
        private readonly IMapper _mapper;
        private readonly IChatHistoryService _chatHistoryService;
        private readonly IGeminiChatService _chatService;
        private readonly ILogger<AIEmbeddingService> _logger;

        public AIEmbeddingService(IUnitOfWork unitOfWork, IGeminiChatService chatService, IMapper mapper, IChatHistoryService chatHistoryService, ILogger<AIEmbeddingService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _chatHistoryService = chatHistoryService;
            _chatService = chatService;
            var apiKey = chatService.GetApiKey();
            _embeddingModel = new EmbeddingModel(apiKey, "text-embedding-004");
            _logger = logger;

        }


       
        /*-------------------------------Gemini Replay-----------------------------------------*/

        //public async Task<string> GetJobAnswerFromGeminiAsync(string userQuestion)
        //{
        //    //use semantic search to pull similar jobs (most top 3 , TopK=3)
        //    var topJobs = await SearchJobsByMeaningAsync(userQuestion, 3);

        //    if (topJobs == null || !topJobs.Any())
        //        return "Sorry, I couldn't find any jobs matching your query.";

        //    var contextBuilder = new StringBuilder();

        //    foreach (var result in topJobs)
        //    {
        //        var job = result.Job;
        //        contextBuilder.AppendLine($"""
        //        Job Title: {job.Title}
        //        Description: {job.Description}
        //        Requirements: {job.Requirements}
        //        Skills: {string.Join(", ", job.Skills)}
        //        Location: {job.CompanyLocation}
        //        """);
        //    }


        //    //prepare the prompt
        //    var finalPrompt = $"""
        //            You are a job assistant. Based on the following job listings, answer the user's question:

        //            {contextBuilder}

        //            User's question: {userQuestion}
        //            """;

        //    var response = await _chatService.AskGeminiAsync(finalPrompt);

        //    return response ?? "Error throgh Generation";
        //}


        /*---------------------------Generate Embeddings Functions-------------------------------*/
        public async Task GenerateEmbeddingsForJobsAsync()
        {
            var jobRepo = _unitOfWork.Repository<Job>();

            // Get all jobs with Employer, Skills, and Categories
            var spec = new JobsWithDetailsSpec();
            var jobs = await jobRepo.GetAllAsync(spec);

            var embeddingRepo = _unitOfWork.Repository<AIEmbedding>();
            var existingEmbeddings = await embeddingRepo.GetAllAsync();

            // Use dictionary for faster lookup
            var embeddingsDict = existingEmbeddings
                .OfType<AIEmbedding>()
                .Where(e => e.EntityType == "Job")
                .ToDictionary(e => e.EntityId, e => e);
            foreach (var job in jobs)
            {
                Console.WriteLine($"Job: {job.Title}");
                Console.WriteLine($"Employer: {job.Employer?.CompanyName}");
                Console.WriteLine($"Location: {job.Employer?.CompanyLocation}");
                var dto = _mapper.Map<JobDto>(job);
                var content = BuildJobContent(dto);

                var response = await _embeddingModel.EmbedContentAsync(content);
                if (response?.Embedding?.Values == null || !response.Embedding.Values.Any())
                    continue;

                var vector = response.Embedding.Values.Select(v => (float)v).ToArray();

                var existingEmbedding = await embeddingRepo.FindAsync(
                    e => e.EntityType == "Job" && e.EntityId == job.Id);

                if (existingEmbedding != null)
                {
                    existingEmbedding.Content = content;
                    existingEmbedding.EmbeddingVector = vector;
                    existingEmbedding.CreatedAt = DateTime.UtcNow;
                    // Update existing embedding
                    _unitOfWork.Repository<AIEmbedding>().Update(existingEmbedding);

                    // Alternatively, you can use:
                    //embeddingRepo.Update(existingEmbedding);
                }
                else
                {
                    var embedding = new AIEmbedding
                    {
                        EntityType = "Job",
                        EntityId = job.Id,
                        Content = content,
                        EmbeddingVector = vector,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.Repository<AIEmbedding>().AddAsync(embedding);
                }
            }

            await _unitOfWork.CompleteAsync();

        }

        // update and generate emebedding for a specific job
        public async Task GenerateEmbeddingForJobAsync(Job job)
        {
            var embeddingRepo = _unitOfWork.Repository<AIEmbedding>();

            // skip if already exists
            var exists = await embeddingRepo.FindAsync(e =>
                           e.EntityType == "Job" && e.EntityId == job.Id);
            if (exists != null) return;

            // Load the job with all related data (Employer, Skills, Categories)
            var jobRepo = _unitOfWork.Repository<Job>();
            var spec = new JobsWithDetailsSpec();
            var jobsWithDetails = await jobRepo.GetAllAsync(spec);
            var jobWithDetails = jobsWithDetails.FirstOrDefault(j => j.Id == job.Id);

            // If job not found with details, use the original job
            if (jobWithDetails == null)
            {
                
                jobWithDetails = job;
            }

            var dto = _mapper.Map<JobDto>(jobWithDetails);
            var content = BuildJobContent(dto);

            var response = await _embeddingModel.EmbedContentAsync(content);
            if (response?.Embedding?.Values == null || !response.Embedding.Values.Any())
                return;

            var vector = response.Embedding.Values.Select(v => (float)v).ToArray();

            // check if embedding already exists
            var existingEmbedding = await embeddingRepo.FindAsync(e =>
                e.EntityType == "Job" && e.EntityId == job.Id);

            if (existingEmbedding != null)
            {
                // update existing embedding
                existingEmbedding.Content = content;
                existingEmbedding.EmbeddingVector = vector;
                existingEmbedding.CreatedAt = DateTime.UtcNow;

                embeddingRepo.Update(existingEmbedding);
            }
            else
            {
                // add new embedding
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

        public async Task DeleteEmbeddingForJobAsync(int jobId)
        {
            var embeddingRepo = _unitOfWork.Repository<AIEmbedding>();

            var existingEmbeddings = await embeddingRepo.FindAsync(e =>
                e.EntityType == "Job" && e.EntityId == jobId);

            if (existingEmbeddings != null)
            {
                embeddingRepo.Delete(existingEmbeddings);
                await _unitOfWork.CompleteAsync();
            }
        }


        /*----------------------------------------------------------------------------------------*/


        /*Find the functions that are closest in meaning to a given word or phrase using embedding.*/

        /* We use the query typed by the user, generate an embedding vector for it,
         * calculate the cosine similarity between it and all the embeddings stored in the database,
         * and then return the top K results as functions that are similar in meaning.*/
        public async Task<List<SemanticSearchResultDto>> SearchJobsByMeaningAsync(string query, int maxResults = 5)
        {

            // Step 1: Generate embedding for the query

            // Step 1: Generate embedding for the query
            var queryResponse = await _embeddingModel.EmbedContentAsync(query);
            if (queryResponse?.Embedding?.Values == null || !queryResponse.Embedding.Values.Any())
            {
                Console.WriteLine($"Failed to generate embedding for query: {query}");
                return new List<SemanticSearchResultDto>();
            }
            var queryVector = queryResponse.Embedding.Values.Select(v => (float)v).ToArray();


            // Step 2: Get all job embeddings
            var embeddingRepo = _unitOfWork.Repository<AIEmbedding>();
            var allEmbeddings = await embeddingRepo.GetAllAsync();
            var jobEmbeddings = allEmbeddings.Where(e => e.EntityType == "Job" && e.EmbeddingVector != null).ToList();
            // Step 3: Compute similarity and filter by threshold
            const float similarityThreshold = 0.6f; // min range of similarity
            var similarities = new List<(AIEmbedding Embedding, float Score)>();

            foreach (var jobEmbedding in jobEmbeddings)
            {
                var score = CosineSimilarity(queryVector, jobEmbedding.EmbeddingVector);
                if (score >= similarityThreshold) // filtered by threshold
                {
                    similarities.Add((jobEmbedding, score));
                    Console.WriteLine($"Job ID: {jobEmbedding.EntityId}, Similarity Score: {score}");
                }
            }


            // Step 4: Order by similarity and take up to maxResults
            var topResults = similarities
                .OrderByDescending(x => x.Score)
                .Take(maxResults)
                .ToList();

            // Load the actual job data and prepare result DTOs
            var jobRepo = _unitOfWork.Repository<Job>();
            var spec = new JobsWithDetailsSpec();
            var allJobs = await jobRepo.GetAllAsync(spec);
            var jobsDict = allJobs.ToDictionary(j => j.Id, j => j);

            // Step 5: Filter results based on keywords in the query
            var keywords = query.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var filteredResults = topResults
                .Where(r => jobsDict.ContainsKey(r.Embedding.EntityId))
                .Where(r =>
                {
                    var job = jobsDict[r.Embedding.EntityId];
                    return keywords.Any(k =>
                        job.Title.ToLower().Contains(k) ||
                        (job.Skills != null && job.Skills.Any(s => s.SkillName.ToLower().Contains(k))));
                })
                .ToList();

            // Step 6: If no filtered results, use top results

            var finalResults = filteredResults.Any() ? filteredResults : topResults;

            // Step 7: Prepare result DTOs
            var resultDtos = new List<SemanticSearchResultDto>();
            foreach (var (embedding, score) in finalResults)
            {
                if (!jobsDict.ContainsKey(embedding.EntityId)) continue;

                var job = jobsDict[embedding.EntityId];
                var dto = _mapper.Map<JobDto>(job);

                resultDtos.Add(new SemanticSearchResultDto
                {
                    Job = dto,
                    Similarity = score
                });
            }

            Console.WriteLine($"Found {resultDtos.Count} relevant jobs for query: {query}");
            return resultDtos;
        }



        /*-----------------------------------Private Functions------------------------------*/

        /* use to create Ebmeddings of job to save it in DB at AIEmbedding Table */
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

                --- Employer Info ---
                Company Name: {job.CompanyName ?? "Not specified"}
                Company Location: {job.CompanyLocation ?? "Not specified"}
                Company Website: {(string.IsNullOrEmpty(job.Website) ? "Not specified" : job.Website)}
                Industry: {job.Industry ?? "Not specified"}
                Company Description: {job.CompanyDescription ?? "Not specified"}
                Company Mission: {job.CompanyMission ?? "Not specified"}
                Employee Range: {job.EmployeeRange ?? "Not specified"}
                Established Year: {job.EstablishedYear?.ToString() ?? "Not specified"}
                """;
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



        /*-------------------------Chat OverLoad Function (Gemini Replay)-------------------------*/
        public async Task<string> GetJobAnswerFromGeminiAsync(string userId, string userQuestion)
        {
            //1) use semantic search to pull similar jobs (most top 3 , TopK=3)
            var topJobs = await SearchJobsByMeaningAsync(userQuestion, 3);
            if (topJobs == null || !topJobs.Any())
                topJobs = new List<SemanticSearchResultDto>();

            // 2) add chat history
            var history = await _chatHistoryService.GetAsync(userId, takeLast: 10);

            //3) prepare the prompt
            // Using existing BuildJobContent method

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
                Education Level: {job.EducationLevel}
                Experience Level: {job.ExperienceLevel}
                Salary: {job.Salary?.ToString() ?? "Not specified"}
                Categories: {string.Join(", ", job.Categories)}
                --- Company Information ---
                Company Name: {job.CompanyName ?? "Not specified"}
                Industry: {job.Industry ?? "Not specified"}
                Company Description: {job.CompanyDescription ?? "Not specified"}
                Company Mission: {job.CompanyMission ?? "Not specified"}
                Employee Range: {job.EmployeeRange ?? "Not specified"}
                Website: {job.Website ?? "Not specified"}
                Established Year: {job.EstablishedYear?.ToString() ?? "Not specified"}
                """);
            }

            //4) build chat history
            var historyBuilder = new StringBuilder();
            foreach (var msg in history)
            {
                historyBuilder.AppendLine($"{msg.Role.ToUpper()}: {msg.Content}");
            }

            // 5) final prompt 
      
            var finalPrompt = $"""
                        You are a specialized job assistant focused on helping users find and learn about job opportunities. Your responses must be concise, relevant, and strictly based on the provided job context.
                        Conversation history (most recent last):
                        {historyBuilder}

                        Job context (use this to answer the question):
                        {contextBuilder}

                        User question:
                        {userQuestion}

                        Instructions:
                        - Always prioritize the job context to answer questions about jobs, salaries, locations, or skills.
                        - For salary questions, explicitly include the salary from the context if available, or state "Salary not specified" if the context says so.
                        - If the context is empty or lacks specific details, provide a general answer based on the question and mention that specific job details are unavailable.
                        - Answer in the same language as the user question (Arabic or English).
                        """;

            // 6) ask from Gemini
            var response = await _chatService.AskGeminiAsync(finalPrompt);
            var aiAnswer = response ?? "Error throgh Generation";

            // 7) save chat history
            await _chatHistoryService.AddAsync(userId, new ChatMessageDto { Role = "user", Content = userQuestion });
            await _chatHistoryService.AddAsync(userId, new ChatMessageDto { Role = "assistant", Content = aiAnswer });

            return aiAnswer;
        }



    }
}
