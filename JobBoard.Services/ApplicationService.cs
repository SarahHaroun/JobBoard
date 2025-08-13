using AutoMapper;
using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Services.Contract;
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Specifications;
using JobBoard.Services.EmployerService;
using JobBoard.Services.SeekerService;
using Microsoft.AspNetCore.Mvc;
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

		public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto createDto, int applicantId)
		{
			//Verify if user has already applied to this job
			var hasApplied = await HasUserAppliedToJobAsync(applicantId, createDto.JobId);
			if (hasApplied)
				return null;

			var application = _mapper.Map<Application>(createDto);
			application.ApplicantId = applicantId;
			application.AppliedDate = DateTime.UtcNow;
			application.Status = ApplicationStatus.Pending;

			var appRepo = _unitOfWork.Repository<Application>();
			await appRepo.AddAsync(application);

			var result = await _unitOfWork.CompleteAsync();
			if (result <= 0)
				return null;

			return _mapper.Map<ApplicationDto>(application);
		}

		public async Task<ApplicationDto> GetApplicationByIdAsync(int id)
		{
			var application = await _unitOfWork.Repository<Application>().GetByIdAsync(id);
			var mappedApplication = _mapper.Map<ApplicationDto>(application);
			return mappedApplication;
		}

		public async Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync()
		{
			var applications = await _unitOfWork.Repository<Application>().GetAllAsync();
		    var mappedApplications = _mapper.Map<IEnumerable<ApplicationDto>>(applications);
			return mappedApplications;

		}

		public async Task<IEnumerable<ApplicationDto>> GetApplicationsWithFilterAsync(ApplicationFilterParams filterParams)
		{
			var spec = new ApplicationWithFilterSpecification(filterParams);
			var applications = await _unitOfWork.Repository<Application>().GetAllAsync(spec);
			var mappedApplications = _mapper.Map<IEnumerable<ApplicationDto>>(applications);
			return mappedApplications;
		}

		public async Task<ApplicationDto> UpdateApplicationStatusAsync(int id, UpdateApplicationStatusDto statusDto)
		{
			var application = await _unitOfWork.Repository<Application>().GetByIdAsync(id);
			if (application == null)
				return null;

			application.Status = statusDto.Status;
			_unitOfWork.Repository<Application>().Update(application);

			var result = await _unitOfWork.CompleteAsync();
			if (result <= 0)
				return null;

			var mappedApplication = _mapper.Map<ApplicationDto>(application);
			return mappedApplication;
		}

		public async Task<bool> DeleteApplicationAsync(int id)
		{
			var application = await _unitOfWork.Repository<Application>().GetByIdAsync(id);
			if (application == null)
				return false;

			_unitOfWork.Repository<Application>().Delete(application);
			var deleted = await _unitOfWork.CompleteAsync();
			return  deleted > 0;
		}

		public async Task<IEnumerable<ApplicationDto>> GetApplicationsByJobIdAsync(int jobId)
		{
			var spec = new ApplicationWithFilterSpecification(jobId);
			var applications = await _unitOfWork.Repository<Application>().GetAllAsync(spec);
			var mappedApplications = _mapper.Map<IEnumerable<ApplicationDto>>(applications);
			return mappedApplications;
		}

		public async Task<IEnumerable<ApplicationDto>> GetApplicationsByApplicantIdAsync(int applicantId)
		{
			var spec = new ApplicationWithFilterSpecification(applicantId);
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
	}
}