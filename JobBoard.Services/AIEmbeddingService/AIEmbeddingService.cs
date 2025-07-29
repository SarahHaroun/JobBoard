using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenerativeAI;
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

        public AIEmbeddingService(IUnitOfWork unitOfWork, IGeminiChatService chatService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

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
    }
}
