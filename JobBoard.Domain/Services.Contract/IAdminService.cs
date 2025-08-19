using JobBoard.Domain.DTO.AdminDto;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.DTO.SeekerDto;

namespace JobBoard.Services.AdminService
{
    public interface IAdminService
    {
        Task<List<SeekerProfileDto>> GetAllSeekersAsync();
        Task<List<EmpProfileDto>> GetAllEmployersAsync();
        Task<List<JobDto>> GetAllJobsAsync();
        Task<List<JobDto>> GetPendingJobsAsync();
        Task<bool> ApproveJobAsync(int jobId);
        Task<bool> RejectJobAsync(int jobId);
        Task<bool> DeleteUserAsync(string userId);
        Task<StatsDto> GetStatsAsync();
    }

}
