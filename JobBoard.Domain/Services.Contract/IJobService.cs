using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO.JobDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Services.Contract
{
    public interface IJobService
    {
        Task<IEnumerable<JobListDto>> GetAllJobsAsync();
        Task<JobDto> GetJobByIdAsync(int id);

        public Task<IEnumerable<JobDto>> GetJobsByCategoryIdAsync(int categoryId);
        public Task<JobDto> AddJobAsync(JobDto jobDto);
    }
}
