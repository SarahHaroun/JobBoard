using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO.JobDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Shared;
using JobBoard.Domain.DTO.CategoryDto;
using JobBoard.Domain.DTO.SkillAndCategoryDto;
using JobBoard.Domain.DTO.JobsDto;
using JobBoard.Domain.DTO.EmployerDto;

namespace JobBoard.Domain.Services.Contract
{
    public interface IJobService
    {
        Task<IEnumerable<JobListDto>> GetAllJobsAsync(JobFilterParams filterParams);
		Task<IEnumerable<PostedJobsDto>> GetEmployerJobsAsync(int employerId, EmployerJobFilterParams filterParams); 
		Task<IEnumerable<TopPerformingJobDto>> GetTopPerformingJobsAsync(int employerId, int limit = 5);
        Task<IEnumerable<RecentJobDto>> GetRecentJobsAsync(int employerId, int limit = 3);
		Task<JobDto> GetJobByIdAsync(int id);
        Task<JobDto> AddJobAsync(CreateUpdateJobDto jobDto, int employerId);
        Task<JobDto> UpdateJob(int id, CreateUpdateJobDto jobDto, int employerId);
        Task<bool> DeleteJob(int id, int employerId);
		Task<EmployerDashboardStatsDto> GetEmployerDashboardStatsAsync(int employerId);
		Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<SkillDto>> GetAllSkillsAsync();

	}
}
