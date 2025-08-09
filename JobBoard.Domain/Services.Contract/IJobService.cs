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

namespace JobBoard.Domain.Services.Contract
{
    public interface IJobService
    {
        Task<IEnumerable<JobListDto>> GetAllJobsAsync(JobFilterParams filterParams);
        Task<JobDto> GetJobByIdAsync(int id);
        Task<JobDto> AddJobAsync(CreateUpdateJobDto jobDto, int employerId);
        Task<JobDto> UpdateJobAsync(int id, CreateUpdateJobDto jobDto);
		Task<bool> DeleteJobAsync(int id);

        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<SkillDto>> GetAllSkillsAsync();
	}
}
