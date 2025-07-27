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

namespace JobBoard.Services
{
	public class JobService : IJobService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public JobService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IEnumerable<JobListDto>> GetAllJobsAsync()
		{
			var jobs = await _unitOfWork.Repository<Job>().GetAllAsync();
			var mappedJobs = _mapper.Map<IEnumerable<JobListDto>>(jobs);
			return mappedJobs;

		}

		public async Task<JobDto> GetJobByIdAsync(int id)
		{
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(id);
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
    }
}
