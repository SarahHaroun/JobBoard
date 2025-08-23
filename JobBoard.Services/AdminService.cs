using AutoMapper;
using JobBoard.Domain.DTO.AdminDto;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Persistence;
using JobBoard.Repositories.Specifications;
using JobBoard.Services.NotificationsService;
using JobBoard.Services.AIEmbeddingService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services.AdminService
{
	public class AdminService : IAdminService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IAIEmbeddingService _aiEmbeddingService;
        private readonly INotificationService _notificationService;

		public AdminService(IUnitOfWork unitOfWork, 
			UserManager<ApplicationUser> userManager,
			IMapper mapper, 
			IAIEmbeddingService aiEmbeddingService,
			INotificationService notificationService)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_mapper = mapper;
			_aiEmbeddingService = aiEmbeddingService;
			_notificationService = notificationService;
        }

		public async Task<List<SeekerProfileDto>> GetAllSeekersAsync()
		{
			var spec = new AllSeekersSpecification();
			var seekers = await _unitOfWork.Repository<SeekerProfile>().GetAllAsync(spec);
			return _mapper.Map<List<SeekerProfileDto>>(seekers);
		}

		public async Task<List<EmpProfileDto>> GetAllEmployersAsync()
		{
			var spec = new AllEmployersSpecification();
			var employers = await _unitOfWork.Repository<EmployerProfile>().GetAllAsync(spec);
			return _mapper.Map<List<EmpProfileDto>>(employers);
		}

		public async Task<List<JobDto>> GetAllJobsAsync()
		{
			var spec = new AllJobsSpecification();
			var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
			return _mapper.Map<List<JobDto>>(jobs);
		}

		public async Task<bool> DeleteJob(int id)
		{
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(id);
			if (job == null)
				return false;

			_unitOfWork.Repository<Job>().Delete(job);
			await _unitOfWork.CompleteAsync();
			await _aiEmbeddingService.DeleteEmbeddingForJobAsync(id);

			return true;
		}

		public async Task<List<JobDto>> GetPendingJobsAsync()
		{
			var spec = new PendingJobsSpecification();
			var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
			return _mapper.Map<List<JobDto>>(jobs);

		}

		public async Task<bool> ApproveJobAsync(int jobId)
		{
			var spec = new JobByIdSpecification(jobId);
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);

			if (job == null) 
				return false;
			//Check if job is already approved
			if (job.IsApproved) 
				return false;

			job.IsApproved = true;
			job.PostedDate = DateTime.Now;
			_unitOfWork.Repository<Job>().Update(job);

			var result = await _unitOfWork.CompleteAsync();

            var notificationMessage = $"Your job {job.Title} has been approved!";
             var jobLink = $"/jobDtl/{job.Id}"; 

            await _notificationService.AddNotificationAsync(job.Employer.UserId,notificationMessage,jobLink);
            return result > 0;
		}

		public async Task<bool> RejectJobAsync(int jobId)
		{
			var spec = new JobByIdSpecification(jobId);
			var job = await _unitOfWork.Repository<Job>().GetByIdAsync(spec);

			if (job == null) return false;

			_unitOfWork.Repository<Job>().Delete(job);

			var result = await _unitOfWork.CompleteAsync();

			var notificationMessage = $"Your job {job.Title} has been rejected!";
			var jobLink = $"/jobDtl/{job.Id}";

			await _notificationService.AddNotificationAsync(job.Employer.UserId, notificationMessage,jobLink);
			await _aiEmbeddingService.DeleteEmbeddingForJobAsync(jobId);
			return result > 0;
		}

		public async Task<bool> DeleteUserAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return false;

			var result = await _userManager.DeleteAsync(user);
			return result.Succeeded;
		}

		public async Task<StatsDto> GetStatsAsync()
		{
			var seekersCount = (await _userManager.GetUsersInRoleAsync(UserType.Seeker.ToString())).Count;
			var employersCount = (await _userManager.GetUsersInRoleAsync(UserType.Employer.ToString())).Count;

			// Using repository for counting jobs
			var jobsCount = await _unitOfWork.Repository<Job>().CountAsync();

			var pendingJobsSpec = new PendingJobsSpecification();
			var pendingJobsCount = await _unitOfWork.Repository<Job>().CountAsync(pendingJobsSpec);

			return new StatsDto()
			{
				TotalSeekers = seekersCount,
				TotalEmployers = employersCount,
				TotalJobs = jobsCount,
				PendingJobs = pendingJobsCount
			};
		}

	}
}