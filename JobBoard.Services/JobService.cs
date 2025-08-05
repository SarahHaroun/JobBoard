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

		public async Task<JobDto> AddJobAsync(CreateUpdateJobDto jobDto, int employerId)
		{
			var job = await CreateJobFromDtoAsync(jobDto, employerId);
			await SaveJobAsync(job);
			await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);
			return _mapper.Map<JobDto>(job);
		}

		public async Task<JobDto> UpdateJobAsync(int id, CreateUpdateJobDto jobDto)
		{
			var job = await GetJobForUpdateAsync(id);
			await UpdateJobFromDtoAsync(job, jobDto);
			await SaveJobAsync(job, isUpdate: true);
			await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);
			return _mapper.Map<JobDto>(job);
		}
		public async Task<bool> DeleteJobAsync(int id)
		{
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(id);

			if (job == null)
				return false;

			_unitOfWork.Repository<Job>().Delete(job);
			await _unitOfWork.CompleteAsync();
			return true;
		}

		// Helper methods
		private async Task<Job> CreateJobFromDtoAsync(CreateUpdateJobDto jobDto, int employerId)
		{
			var job = _mapper.Map<Job>(jobDto);
			job.EmployerId = employerId;
			await MapRelationshipsAsync(job, jobDto);
			return job;
		}

		private async Task<Job> GetJobForUpdateAsync(int id)
		{
			var spec = new JobsWithFilterSpecifications(id);
			return await _unitOfWork.Repository<Job>().GetByIdAsync(spec);
		}

		private async Task UpdateJobFromDtoAsync(Job job, CreateUpdateJobDto jobDto)
		{
			_mapper.Map(jobDto, job);
			await MapRelationshipsAsync(job, jobDto);
		}

		private async Task MapRelationshipsAsync(Job job, CreateUpdateJobDto jobDto)
		{
			await MapSkillsAsync(job, jobDto.SkillIds);
			await MapCategoriesAsync(job, jobDto.CategoryIds);
		}

		private async Task MapSkillsAsync(Job job, List<int>? skillIds)
		{
			job.Skills.Clear();
			if (skillIds?.Any() == true)
			{
				var skills = await GetEntitiesByIdsAsync<Skill>(skillIds);
				job.Skills = skills.ToList();
			}
		}

		private async Task MapCategoriesAsync(Job job, List<int>? categoryIds)
		{
			job.Categories.Clear();
			if (categoryIds?.Any() == true)
			{
				var categories = await GetEntitiesByIdsAsync<Category>(categoryIds);
				job.Categories = categories.ToList();
			}
		}

		private async Task<IEnumerable<T>> GetEntitiesByIdsAsync<T>(List<int> ids) where T : class
		{
			var allEntities = await _unitOfWork.Repository<T>().GetAllAsync();
			return allEntities.Where(e => ids.Contains(GetEntityId(e)));
		}

		private int GetEntityId<T>(T entity)
		{
			// Assuming all entities have an Id property
			return (int)entity.GetType().GetProperty("Id").GetValue(entity);
		}

		private async Task SaveJobAsync(Job job, bool isUpdate = false)
		{
			if (isUpdate)
			{
				_unitOfWork.Repository<Job>().Update(job);
			}
			else
			{
				await _unitOfWork.Repository<Job>().AddAsync(job);
			}
			await _unitOfWork.CompleteAsync();
		}

	}
}
