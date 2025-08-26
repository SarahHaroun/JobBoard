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
using JobBoard.Repositories.Specifications.JobSpecifications;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Repositories.Specifications.DashboardSpecifications;
using JobBoard.Repositories.Redis;

namespace JobBoard.Services
{
	public class JobService : IJobService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IAIEmbeddingService _aiEmbeddingService;
        private readonly IRedisService _redisService;

        public JobService(IUnitOfWork unitOfWork, IMapper mapper , IAIEmbeddingService aiEmbeddingService ,IRedisService redisService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _aiEmbeddingService = aiEmbeddingService;
            _redisService = redisService;
        }
		public async Task<IEnumerable<JobListDto>> GetAllJobsAsync(JobFilterParams filterParams)
		{
            var spec = new JobsWithFilterSpecifications(filterParams);
			var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
			var mappedJobs = _mapper.Map<IEnumerable<JobListDto>>(jobs);
			return mappedJobs;

		}
		public async Task<IEnumerable<PostedJobsDto>> GetEmployerJobsAsync(int employerId, EmployerJobFilterParams filterParams)
		{
			var spec = new EmployerJobsWithFilterSpecification(employerId, filterParams);
			var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
			var mappedJobs = _mapper.Map<IEnumerable<PostedJobsDto>>(jobs);

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
            

			await _redisService.DeleteByPrefixAsync("jobs:");

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
            await _redisService.DeleteByPrefixAsync("jobs:");

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
            await _redisService.DeleteByPrefixAsync("jobs:");

            return true;
		}

		public async Task<EmployerDashboardStatsDto> GetEmployerDashboardStatsAsync(int employerId)
		{
			var currDate = DateTime.Now;
			var startOfMonth = new DateTime(currDate.Year, currDate.Month, 1);

			// Active Jobs
			var activeJobsSpec = new ActiveJobsSpecification(employerId);
			var activeJobs = await _unitOfWork.Repository<Job>().CountAsync(activeJobsSpec);

			// Applications this month  
			var applicationsSpec = new ApplicationsMonthSpecification(employerId, startOfMonth);
			var applicationsThisMonth = await _unitOfWork.Repository<Application>().CountAsync(applicationsSpec);

			// Jobs expiring soon
			var expiringSoonSpec = new JobsExpiringSoonSpecification(employerId, currDate.AddDays(7));
			var jobsExpiringSoon = await _unitOfWork.Repository<Job>().CountAsync(expiringSoonSpec);

			return new EmployerDashboardStatsDto
			{
				TotalActiveJobs = activeJobs,
				ApplicationsThisMonth = applicationsThisMonth,
				JobsExpiringSoon = jobsExpiringSoon
			};
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
