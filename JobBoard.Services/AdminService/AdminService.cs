using AutoMapper;
using JobBoard.Domain.DTO.AdminDto;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Repositories.Persistence;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdminService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<SeekerProfileDto>> GetAllSeekersAsync()
        {
            var seekers = await _context.SeekerProfiles
              .Include(s => s.User) 
              .ToListAsync();
            return _mapper.Map<List<SeekerProfileDto>>(seekers);
        }

        public async Task<List<EmpProfileDto>> GetAllEmployersAsync()
        {
            var employers = await _context.EmployerProfiles
             .Include(s => s.User) 
             .ToListAsync();
            return _mapper.Map<List<EmpProfileDto>>(employers);
        }

        public async Task<List<JobDto>> GetAllJobsAsync()
        {
            var jobs = await _context.Jobs
             .Include(j => j.Employer)
             .ToListAsync();
            return _mapper.Map<List<JobDto>>(jobs);
        }

        public async Task<List<JobDto>> GetPendingJobsAsync()
        {

            var jobs = await _context.Jobs
             .Include(j => j.Employer)
             .Where(j => !j.IsApproved)
             .ToListAsync();
            return _mapper.Map<List<JobDto>>(jobs);

        }

        public async Task<bool> ApproveJobAsync(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null) return false;

            job.IsApproved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectJobAsync(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null) return false;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return true;
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
            var jobsCount = await _context.Jobs.CountAsync();
            var pendingJobsCount = await _context.Jobs.CountAsync(j => !j.IsApproved);

            return new StatsDto() 
            {
                TotalSeekers= seekersCount,
                TotalEmployers= employersCount,
                TotalJobs= jobsCount,
                PendingJobs= pendingJobsCount
            };
        }
    }

}
