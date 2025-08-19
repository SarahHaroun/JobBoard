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
using JobBoard.Domain.DTO.CategoryDto;
using JobBoard.Domain.DTO.SkillAndCategoryDto;
using JobBoard.Domain.DTO.JobsDto;

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

		public async Task<IEnumerable<TopPerformingJobDto>> GetTopPerformingJobsAsync(int employerId, int limit = 5)
		{
			var spec = new TopPerformingJobsSpecification(employerId, limit);
			var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
			var mappedJobs = _mapper.Map<IEnumerable<TopPerformingJobDto>>(jobs);
			return mappedJobs;
		}

		public async Task<IEnumerable<RecentJobDto>> GetRecentJobsAsync(int employerId, int limit = 3)
		{
			var spec = new RecentJobsSpecification(employerId, limit);
			var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
			var mappedJobs = _mapper.Map<IEnumerable<RecentJobDto>>(jobs);
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
			var job = _mapper.Map<Job>(jobDto);
			job.EmployerId = employerId;
			job.IsApproved = false;

			await MapSkillsAndCategoriesAsync(job, jobDto);

			await _unitOfWork.Repository<Job>().AddAsync(job);
			await _unitOfWork.CompleteAsync();

			await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);

			return _mapper.Map<JobDto>(job);
		}

		public async Task<JobDto> UpdateJob(int id, CreateUpdateJobDto jobDto, int employerId)
		{
			var spec = new JobUpdateSpecification(id, employerId);
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);

			if (job == null)
				return null;

			_mapper.Map(jobDto, job);

			if (job.IsApproved)
			{
				job.IsApproved = false;
			}

			await MapSkillsAndCategoriesAsync(job, jobDto);
			_unitOfWork.Repository<Job>().Update(job);
			await _unitOfWork.CompleteAsync();
			await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);

			return _mapper.Map<JobDto>(job);
		}

		public async Task<bool> DeleteJob(int id, int employerId)
		{
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(id);
			if (job == null)
				return false;

			if (job.EmployerId != employerId)
				return false;

			_unitOfWork.Repository<Job>().Delete(job);
			await _unitOfWork.CompleteAsync();
            await _aiEmbeddingService.DeleteEmbeddingForJobAsync(id);

            return true;
		}

		private async Task MapSkillsAndCategoriesAsync(Job job, CreateUpdateJobDto jobDto)
		{
		
			// Categories
			if (job.Categories == null)
				job.Categories = new List<Category>();

			job.Categories.Clear();

			if (jobDto.CategoryIds?.Count > 0)
			{
				var spec = new CategoriesByIdsSpecifications(jobDto.CategoryIds);
				var categories = await _unitOfWork.Repository<Category>().GetAllAsync(spec);
				job.Categories = categories.ToList();
			}
			// Skills
			if (job.Skills == null)
				job.Skills = new List<Skill>();
			job.Skills.Clear();

			if (jobDto.SkillIds?.Count > 0)
			{
				var spec = new SkillsByIdsSpecifications(jobDto.SkillIds);
				var skills = await _unitOfWork.Repository<Skill>().GetAllAsync(spec);
				job.Skills = skills.ToList();
			}
		}

		public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
		{
			var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
			var mappedCategories = _mapper.Map<IEnumerable<CategoryDto>>(categories);

			return mappedCategories;
		}

		public async Task<IEnumerable<SkillDto>> GetAllSkillsAsync()
		{
			var skills  = await _unitOfWork.Repository<Skill>().GetAllAsync();
			var mappedSkills = _mapper.Map<IEnumerable<SkillDto>>(skills);

			return mappedSkills;
		}
	}
}
