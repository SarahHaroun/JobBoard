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
			var job = _mapper.Map<Job>(jobDto);
			job.EmployerId = employerId;

			await MapSkillsAndCategoriesAsync(job, jobDto);

			await _unitOfWork.Repository<Job>().AddAsync(job);
			await _unitOfWork.CompleteAsync();

			await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);

			return _mapper.Map<JobDto>(job);
		}

		public async Task<JobDto> UpdateJobAsync(int id, CreateUpdateJobDto jobDto)
		{
			var spec = new JobsWithFilterSpecifications(id);
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);

			if (job == null)
				return null;

			_mapper.Map(jobDto, job);

			await MapSkillsAndCategoriesAsync(job, jobDto);

			_unitOfWork.Repository<Job>().Update(job);
			await _unitOfWork.CompleteAsync();

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
            await _aiEmbeddingService.DeleteEmbeddingForJobAsync(id);

            return true;
		}

		private async Task MapSkillsAndCategoriesAsync(Job job, CreateUpdateJobDto jobDto)
		{
		
			if (job.Categories == null)
				job.Categories = new List<Category>();

			if (job.Skills == null)
				job.Skills = new List<Skill>();

			// Categories
			job.Categories.Clear();

			if (jobDto.CategoryIds != null && jobDto.CategoryIds.Count > 0)
			{
				var allCategories = await _unitOfWork.Repository<Category>().GetAllAsync();
				List<Category> selectedCategories = new List<Category>();

				foreach (var category in allCategories)
				{
					if (jobDto.CategoryIds.Contains(category.Id))
					{
						selectedCategories.Add(category);
					}
				}

				job.Categories = selectedCategories;
			}
			// Skills
			job.Skills.Clear();

			if (jobDto.SkillIds != null && jobDto.SkillIds.Count > 0)
			{
				var allSkills = await _unitOfWork.Repository<Skill>().GetAllAsync();
				List<Skill> selectedSkills = new List<Skill>();

				foreach (var skill in allSkills)
				{
					if (jobDto.SkillIds.Contains(skill.Id))
					{
						selectedSkills.Add(skill);
					}
				}

				job.Skills = selectedSkills;
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
