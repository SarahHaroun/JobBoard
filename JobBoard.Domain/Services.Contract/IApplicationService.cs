using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
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
		//SEEKER METHODS
		Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto createDto, int applicantId);
		Task<IEnumerable<SeekerApplicationListDto>> GetApplicationsByApplicantIdAsync(int applicantId);
		Task<bool> HasUserAppliedToJobAsync(int applicantId, int jobId);

		//EMPLOYER METHODS
		Task<IEnumerable<EmployerApplicationListDto>> GetApplicationsForEmployerJobsAsync(int employerId, ApplicationFilterParams filterParams);
		Task<IEnumerable<EmployerApplicationListDto>> GetApplicationsByJobIdAsync(int jobId, int employerId);
		Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, int employerId);
		
		//ADMIN METHODS
		Task<bool> DeleteApplicationAsync(int id);
		//MUTUAL METHODS
		Task<ApplicationDto> GetApplicationByIdAsync(int id);

	}

}

