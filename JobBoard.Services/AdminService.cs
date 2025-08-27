using AutoMapper;
using JobBoard.Domain.DTO.AdminDto;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Shared;
using JobBoard.Repositories;
using JobBoard.Repositories.Persistence;
using JobBoard.Repositories.Specifications;
using JobBoard.Services.AIEmbeddingService;
using JobBoard.Services.NotificationsService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Repositories.Redis;
using Microsoft.AspNetCore.OutputCaching;

namespace JobBoard.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAIEmbeddingService _aiEmbeddingService;
        private readonly INotificationService _notificationService;
        private readonly IRedisService _redisService;
        private readonly IOutputCacheStore _outputCacheStore;

        public AdminService(IUnitOfWork unitOfWork, 
			UserManager<ApplicationUser> userManager,
			IMapper mapper, 
			IAIEmbeddingService aiEmbeddingService,
			INotificationService notificationService,
            IRedisService redisService,
            IOutputCacheStore outputCacheStore)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_mapper = mapper;
			_aiEmbeddingService = aiEmbeddingService;
			_notificationService = notificationService;
            _redisService = redisService;
            _outputCacheStore = outputCacheStore;
        }

        ///////////////////////get all seekers///////////////////////
        public async Task<List<SeekerProfileDto>> GetAllSeekersAsync()
        {
            var spec = new AllSeekersSpecification();
            var seekers = await _unitOfWork.Repository<SeekerProfile>().GetAllAsync(spec);
            return _mapper.Map<List<SeekerProfileDto>>(seekers);
        }

        ///////////////////////get all employers///////////////////////
        public async Task<List<EmpProfileDto>> GetAllEmployersAsync()
        {
            var spec = new AllEmployersSpecification();
            var employers = await _unitOfWork.Repository<EmployerProfile>().GetAllAsync(spec);
            return _mapper.Map<List<EmpProfileDto>>(employers);
        }

        ///////////////////////get seeker by id///////////////////////
        public async Task<SeekerProfileDto> GetSeekerByIdAsync(string userId)
        {
            var spec = new SeekerByUserIdSpecification(userId);
            var seeker = await _unitOfWork.Repository<SeekerProfile>().GetByIdAsync(spec);
            return _mapper.Map<SeekerProfileDto>(seeker);
        }


        ///////////////////////get employer by id///////////////////////
        
        public async Task<EmpProfileDto> GetEmployerByIdAsync(string userId)
        {
            var spec = new EmployerByUserIdSpecification(userId);
            var employer = await _unitOfWork.Repository<EmployerProfile>().GetByIdAsync(spec);
            return _mapper.Map<EmpProfileDto>(employer);
        }


        ///////////////////////get all jobs///////////////////////
        public async Task<List<JobDto>> GetAllJobsAsync()
        {
            var spec = new AllJobsSpecification();
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
            return _mapper.Map<List<JobDto>>(jobs);
        }


        ////////////////////////delete job by id///////////////////////
        public async Task<bool> DeleteJob(int id)
        {
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(new JobByIdWithApplication(id));
            if (job == null)
                return false;

            foreach (var app in job.JobApplications)
            {
                _unitOfWork.Repository<Application>().Delete(app);
            }

            _unitOfWork.Repository<Job>().Delete(job);
            await _unitOfWork.CompleteAsync();
            await _aiEmbeddingService.DeleteEmbeddingForJobAsync(id);
            //await _redisService.DeleteByPrefixAsync("jobs:");
            //await _outputCacheStore.EvictByTagAsync("jobs", default);

            return true;
        }


        ///////////////////////get all pending jobs///////////////////////
        public async Task<List<JobDto>> GetPendingJobsAsync()
        {
            var spec = new PendingJobsSpecification();
            var jobs = await _unitOfWork.Repository<Job>().GetAllAsync(spec);
            return _mapper.Map<List<JobDto>>(jobs);

        }


        ///////////////////////approve job by id///////////////////////
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

            //remove job from redis cache
            await _aiEmbeddingService.GenerateEmbeddingForJobAsync(job);
            await _redisService.DeleteByPrefixAsync("jobs:");
            //await _outputCacheStore.EvictByTagAsync("jobs", default);


            await _notificationService.AddNotificationAsync(job.Employer.UserId,notificationMessage,jobLink);
            return result > 0;
        }



        //////////////////////reject job by id///////////////////////
        public async Task<bool> RejectJobAsync(int jobId)
        {
            var job = await _unitOfWork.Repository<Job>().GetByIdAsync(new JobByIdWithApplication(jobId));

            if (job == null) return false;
            foreach (var app in job.JobApplications)
            {
                _unitOfWork.Repository<Application>().Delete(app);
            }

            _unitOfWork.Repository<Job>().Delete(job);

            var result = await _unitOfWork.CompleteAsync();
            if (job.Employer != null)
            {
                var notificationMessage = $"Your job {job.Title} has been rejected!";
                await _notificationService.AddNotificationAsync(job.Employer.UserId, notificationMessage);
            }

            await _aiEmbeddingService.DeleteEmbeddingForJobAsync(jobId);
            return result > 0;
        }

        //////////////////////delete user by id///////////////////////
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            if (user.User_Type == UserType.Seeker)
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }

            else if (user.User_Type == UserType.Employer)
            {
                var employerSpec = new EmployerByUserIdSpecification(userId);
                var employer = await _unitOfWork.Repository<EmployerProfile>().GetByIdAsync(employerSpec);

                if (employer != null)
                {
                    employer.IsDeleted = true;

                    var jobs = await _unitOfWork.Repository<Job>()
                        .GetAllAsync(new JobsByEmployerIdSpecification(employer.Id));

                    foreach (var job in jobs)
                    {
                        job.IsDeleted = true;
                        await _aiEmbeddingService.DeleteEmbeddingForJobAsync(job.Id);

                        foreach (var app in job.JobApplications)
                        {
                            _unitOfWork.Repository<Application>().Delete(app);  
                        }
                    }

                    _unitOfWork.Repository<EmployerProfile>().Update(employer);
                    await _unitOfWork.CompleteAsync();
                }

                var deleteResult = await _userManager.DeleteAsync(user);
                await _redisService.DeleteByPrefixAsync("admin:");

                return deleteResult.Succeeded;
            }

            return false;
        }

        //////////////////////get stats///////////////////////
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

		
        //////////////////////get public stats for home page///////////////////////
        public async Task<PublicStatsDto> GetPublicStatsAsync()
        {
            var fullStats = await GetStatsAsync();
			var approvedJobsCount = await _unitOfWork.Repository<Job>()
					.CountAsync(new ApprovedJobsCountSpecification());
            var activeJobsCount = await _unitOfWork.Repository<Job>()
					.CountAsync(new ActiveJobsCountSpecification());

			return new PublicStatsDto()
            {
                TotalSeekers = fullStats.TotalSeekers,
                TotalEmployers = fullStats.TotalEmployers,
                TotalJobs = fullStats.TotalJobs,
                ApprovedJobs = approvedJobsCount,
                ActiveJobs = activeJobsCount
            };
        }


		//////////////////////get active users count (seekers + employers)///////////////////////
		public async Task<int> GetActiveUsersCountAsync()
		{
			var stats = await GetStatsAsync();
			return stats.TotalSeekers + stats.TotalEmployers;
		}

      

    }
}