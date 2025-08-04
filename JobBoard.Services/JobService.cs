using AutoMapper;
using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Services.AIEmbeddingService;
using JobBoard.Repositories.Specifications;
using JobBoard.Domain.Shared;

namespace JobBoard.Services
{
	public class JobService : IJobService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IAIEmbeddingService _aiEmbeddingService;
        public JobService(IUnitOfWork unitOfWork, IMapper mapper , IAIEmbeddingService aiEmbeddingService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _aiEmbeddingService = aiEmbeddingService;
        }
		public async Task<IEnumerable<JobListDto>> GetAllJobsAsync(JobFilterParams filterParams)
		{
            var spec = new JobsWithFilterSpecifications(filterParams);
			var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
			var mappedJobs = _mapper.Map<IEnumerable<JobListDto>>(jobs);
			return mappedJobs;

		}



		public async Task<JobDto> GetJobByIdAsync(int id)
		{
			var spec = new JobsWithFilterSpecifications(id);
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);
			var mappedJob = _mapper.Map<JobDto>(job);
			return mappedJob;
		}


        public async Task<IEnumerable<JobDto>> GetJobsByCategoryIdAsync(int categoryId)
        {
            // Get all jobs (includes Category navigation property)
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync();

            // Filter by categoryId
            var filteredJobs = jobs
                .Where(j => j.Categories.Any(c => c.Id == categoryId));

            // Map to DTO
            var jobDtos = _mapper.Map<IEnumerable<JobDto>>(filteredJobs);

            return jobDtos;
        }

        public async Task<JobDto> AddJobAsync(JobDto jobDto)
        {
            var job = _mapper.Map<Job>(jobDto);

            // Add the job to the database
            await _unitOfWork.Repository<Job>().AddAsync(job);
            await _unitOfWork.CompleteAsync();

            // Generate embedding for the new job
            await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);

            // Map the saved job back to DTO and return
            return _mapper.Map<JobDto>(job);
        }


    }
}
