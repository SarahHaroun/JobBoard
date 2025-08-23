using AutoMapper;
using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Services.Contract;
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Specifications;
using JobBoard.Repositories.Specifications.ApplicationSpecifications;
using JobBoard.Services.EmployerService;
using JobBoard.Services.NotificationsService;
using JobBoard.Services.SeekerService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services
{
	public class ApplicationService : IApplicationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IConfiguration _configuration;
		private readonly INotificationService _notificationService;


		public ApplicationService(IUnitOfWork unitOfWork,
			IMapper mapper,
			IWebHostEnvironment env,
			IConfiguration configuration,
			INotificationService notificationService)

		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_env = env;
			_configuration = configuration;
			_notificationService = notificationService;
		}
		public async Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto createDto, int applicantId)
		{
			//Verify if user has already applied to this job
			var hasApplied = await HasUserAppliedToJobAsync(applicantId, createDto.JobId);
			if (hasApplied)
				return null;


			// Get the job details to access employer information
			var spec = new Repositories.Specifications.ApplicationSpecifications.JobByIdSpecification(createDto.JobId);
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);
			if (job == null)
				return null;

			var application = _mapper.Map<Application>(createDto);
			application.ApplicantId = applicantId;
			application.AppliedDate = DateTime.UtcNow;
			application.Status = ApplicationStatus.Pending;

			if (createDto.ResumeUrl != null && createDto.ResumeUrl.Length > 0)
			{
				application.ResumeUrl = await DocumentSettings.UploadFileAsync(createDto.ResumeUrl, "cv", _env, _configuration);
			}

			await _unitOfWork.Repository<Application>().AddAsync(application);

			var result = await _unitOfWork.CompleteAsync();
			if (result <= 0)
				return null;


			// Send notification to employer after successful save
			await SendApplicationNotificationAsync(job, createDto.FullName, application.Id);

			return _mapper.Map<ApplicationDto>(application);

		}
		public async Task<ApplicationDto> GetApplicationByIdAsync(int id)
		{
			var spec = new ApplicationWithFilterSpecification(id);
			var applications = await _unitOfWork.Repository<Application>().GetAllAsync(spec);
			var application = applications.FirstOrDefault();
			var mappedApplication = _mapper.Map<ApplicationDto>(application);
			return mappedApplication;
		}

		public async Task<IEnumerable<ApplicationDto>> GetApplicationsByApplicantIdAsync(int applicantId)
		{
			var spec = new ApplicationWithFilterSpecification(applicantId, isApplicantId: true);
			var applications = await _unitOfWork.Repository<Application>().GetAllAsync(spec);
			var mappedApplications = _mapper.Map<IEnumerable<ApplicationDto>>(applications);
			return mappedApplications;
		}

		public async Task<bool> HasUserAppliedToJobAsync(int applicantId, int jobId)
		{
			var spec = new ApplicationWithFilterSpecification(applicantId, jobId);
			var existing = await _unitOfWork.Repository<Application>().GetAllAsync(spec);
			return existing.Any();
		}

		// Method for getting applications for employer's jobs
		public async Task<IEnumerable<EmployerApplicationListDto>> GetApplicationsForEmployerJobsAsync(int employerId, ApplicationFilterParams filterParams)
		{
			var spec = new ApplicationForEmployerJobsSpecification(employerId, filterParams);
			var applications = await _unitOfWork.Repository<Application>().GetAllAsync(spec);
			var mappedApplications = _mapper.Map<IEnumerable<EmployerApplicationListDto>>(applications);
			return mappedApplications;
		}

		// Method for updating application status
		public async Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, int employerId)
		{
			// Retrieve the application with job information
			var spec = new ApplicationWithFilterSpecification(applicationId);
			var applications = await _unitOfWork.Repository<Application>().GetAllAsync(spec);
			var application = applications.FirstOrDefault();

			// Validate application existence and employer ownership
			if (application == null || application.Job?.EmployerId != employerId)
				return false;

			// Check if the status is actually changing
			var oldStatus = application.Status;
			if (oldStatus == status)
				return false;

			// Update status
			application.Status = status;
			_unitOfWork.Repository<Application>().Update(application);
			var result = await _unitOfWork.CompleteAsync();

			if (result > 0)
			{
				// Generate notification message
				var notificationMessage = GetNotificationMessage(status, application.Job?.Title);
				var applicationLink = $"/applicationDtl/{application.Id}";

				// Send notification to the applicant
				await _notificationService.AddNotificationAsync(
					application.Applicant.UserId,
					notificationMessage,
					applicationLink
				);
			}

			return result > 0;
		}


		public async Task<bool> DeleteApplicationAsync(int id)
		{
			var application = await _unitOfWork.Repository<Application>().GetByIdAsync(id);
			if (application == null)
				return false;

			_unitOfWork.Repository<Application>().Delete(application);

			var deleted = await _unitOfWork.CompleteAsync();
			return deleted > 0;
		}

		private async Task SendApplicationNotificationAsync(Job job, string applicantName, int applicationId)
		{
			var notificationMessage = $"{applicantName} has applied for the job: {job.Title}";
			var applicationLink = $"/appView/{applicationId}";
			await _notificationService.AddNotificationAsync(job.Employer.UserId, notificationMessage, applicationLink);
		}

		private string GetNotificationMessage(ApplicationStatus status, string jobTitle)
		{
			switch (status)
			{
				case ApplicationStatus.Accepted:
					return $"🎉 Congratulations! Your application for '{jobTitle}' has been accepted.";

				case ApplicationStatus.Rejected:
					return $"🙏 Thank you for your interest. Your application for '{jobTitle}' was not selected at this time.";

				case ApplicationStatus.UnderReview:
					return $"🔎 Your application for '{jobTitle}' is now under review.";

				case ApplicationStatus.Interviewed:
					return $"📅 You have been selected for an interview for '{jobTitle}'.";

				default:
					return $"Your application for '{jobTitle}' has been updated.";
			}
		}

	}
}