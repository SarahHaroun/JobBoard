using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Services.Contract
{
	public interface IApplicationService
	{
		Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto createDto, int applicantId);
		Task<ApplicationDto> GetApplicationByIdAsync(int id);
		Task<IEnumerable<ApplicationDto>> GetApplicationsByApplicantIdAsync(int applicantId);
		Task<bool> HasUserAppliedToJobAsync(int applicantId, int jobId);
		Task<bool> DeleteApplicationAsync(int id);

	}

}