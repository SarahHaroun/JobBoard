using JobBoard.Domain.DTO.SavedJobsDto;
using JobBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Services.Contract
{
    public interface ISavedJobService
    {

		Task<IEnumerable<SavedJobDto>> GetSavedJobsAsync(int seekerId, SavedJobFilterParams filterParams);
		Task<SavedJobDto> GetSavedJobByIdAsync(int seekerId, int jobId);
		Task<SavedJobDto> SaveJobAsync(int seekerId, CreateSavedJobDto createSavedJobDto);
		Task<bool> UnsaveJobAsync(int seekerId, int jobId);
		Task<bool> IsJobSavedAsync(int seekerId, int jobId);
	}
}
