using AutoMapper;
using JobBoard.Domain.DTO;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services.EmployerService
{
	public class EmployerService : IEmployerService
	{
		private readonly ApplicationDbContext context;
		private readonly IMapper mapper;
		private readonly IWebHostEnvironment env;
		private readonly IConfiguration configuration;

		public EmployerService(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment env, IConfiguration configuration)
		{
			this.context = context;
			this.mapper = mapper;
			this.env = env;
			this.configuration = configuration;
		}

        /////////////////////////create employer profile///////////////////////

        //public async Task<string> Create(EmpProfileDto model)
        //{
        //    //check if employer exist
        //    var existUser = await context.EmployerProfiles.FirstOrDefaultAsync(u => u.UserId == model.UserId);
        //    if (existUser != null)
        //        return "Employer profile already exists";

        //    //if the employer not exist before, create new profile
        //    var newEmployer = mapper.Map<EmployerProfile>(model);

        //    context.EmployerProfiles.Add(newEmployer);
        //    await context.SaveChangesAsync();

        //    return "Employer created successfully";
        //}


        /////////////////////////delete employer profile///////////////////////
        public async Task<bool> DeleteById(int id)
		{
			var employer = await context.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == id);
			if (employer == null)
			{
				return false;
			}
			var defaultCompanyImage = $"{configuration["ApiBaseUrl"]}/images/companies/default.jpg";


			// Delete company image only if it's not the default
			if (!string.IsNullOrEmpty(employer.CompanyImage) && !FileUploadHelper.IsDefaultImage(employer.CompanyImage, defaultCompanyImage))
			{
				DocumentSettings.DeleteFile(employer.CompanyImage, "images/companies", env);
			}
			context.EmployerProfiles.Remove(employer);
			await context.SaveChangesAsync();
			return true;
		}


        ////////////////////////get all employers///////////////////////
        public async Task<List<EmpProfileDto>> GetAll()
		{
			var employers = await context.EmployerProfiles.Include(e => e.User).ToListAsync();

			//var empDtoList = new List<EmpProfileDto>();

			//foreach (var emp in employers)
			//{
			//    empDtoList.Add(new EmpProfileDto
			//    {
			//        Id = emp.Id,
			//        CompanyName = emp.CompanyName,
			//        CompanyLocation = emp.CompanyLocation,
			//        UserId = emp.UserId
			//    });
			//}
			//return empDtoList;
			return mapper.Map<List<EmpProfileDto>>(employers);

		}


        ////////////////////////update employer profile///////////////////////
        public async Task<bool> Update(int id, EmpProfileUpdateDto model)
		{
			var employer = await context.EmployerProfiles.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);
			if (employer == null)
				return false;

			// ================= Profile Image =================
			var defaultCompanyImage = $"{configuration["ApiBaseUrl"]}/images/companies/default.jpg";

			// Handle Company Image upload/removal with default
			employer.CompanyImage = await FileUploadHelper.HandleFileUploadAsync(
				model.CompanyImage, employer.CompanyImage, "images/companies", env,
				configuration, model.RemoveCompanyImage, defaultCompanyImage);

			// Update the employer profile using AutoMapper
			mapper.Map(model, employer);

			// Update related User entity manually if needed
			if (!string.IsNullOrEmpty(model.Phone))
			{
				employer.User.PhoneNumber = model.Phone;
			}
			context.EmployerProfiles.Update(employer);
			await context.SaveChangesAsync();
			return true;
		}

        ////////////////////////get employer by user id///////////////////////
        public async Task<EmpProfileDto?> GetByUserId(string userId)
		{
			var emp = await context.EmployerProfiles.Include(e => e.User)
				.Include(e => e.PostedJobs).ThenInclude(js => js.Skills).FirstOrDefaultAsync(e => e.UserId == userId);
			if (emp == null)
				return null;

			return mapper.Map<EmpProfileDto>(emp);
		}



        ////////////////////////get employer dashboard stats///////////////////////
        public async Task<AnalyticsEmployerDto> GetDashboardStats(int employerId)
        {
            //if (string.IsNullOrEmpty(employerId)) throw new ArgumentNullException(nameof(employerId));
            if (!await context.EmployerProfiles.AnyAsync(e => e.Id == employerId)) throw new ArgumentException("Invalid employer ID", nameof(employerId));

            var now = DateTime.UtcNow;
            var lastMonthStart = now.AddMonths(-1).Date;
            var lastMonthEnd = now.Date;
            var lastQuarterStart = now.AddMonths(-3).Date;
            var lastQuarterEnd = lastMonthStart;

            // 1. Application Rate
            var thisMonthJobs = await context.Jobs
                .Where(j => j.EmployerId == employerId && j.IsApproved && j.PostedDate >= lastMonthStart && j.PostedDate < now)
                .CountAsync();

            var thisMonthApps = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.AppliedDate >= lastMonthStart && a.AppliedDate < now)
                .CountAsync();

            var applicationRate = thisMonthJobs > 0 ? (double)thisMonthApps / thisMonthJobs * 100 : 0;

            var prevMonthJobs = await context.Jobs
                .Where(j => j.EmployerId == employerId && j.IsApproved && j.PostedDate >= lastQuarterEnd.AddMonths(-1) && j.PostedDate < lastMonthStart)
                .CountAsync();

            var prevMonthApps = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.AppliedDate >= lastQuarterEnd.AddMonths(-1) && a.AppliedDate < lastMonthStart)
                .CountAsync();

            var prevApplicationRate = prevMonthJobs > 0 ? (double)prevMonthApps / prevMonthJobs * 100 : 0;
            var applicationRateChange = applicationRate - prevApplicationRate;

            // 2. Profile Views
            var profileViews = thisMonthApps; // Assuming each application represents a profile view
            var prevProfileViews = prevMonthApps; // Same assumption for previous month
            var profileViewsChange = profileViews - prevProfileViews;

            // 3. Time to Hire
            var hires = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.Status== ApplicationStatus.Accepted  && a.Job.IsApproved && a.Job.PostedDate != null)
                .Select(a => (a.AppliedDate - a.Job.PostedDate).Days)
                .ToListAsync();

            var timeToHire = hires.Any() ? (int)hires.Average() : 0;

            // 4. Hire Success Rate
            var thisQuarterApps = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.AppliedDate  >= lastQuarterEnd && a.AppliedDate < now)
                .ToListAsync();

            var totalThisQuarter = thisQuarterApps.Count;
            var successfulThisQuarter = thisQuarterApps.Count(a => a.Status == ApplicationStatus.Accepted);
            var hireSuccessRate = totalThisQuarter > 0 ? (double)successfulThisQuarter / totalThisQuarter * 100 : 0;

            var prevQuarterApps = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.AppliedDate >= lastQuarterStart.AddMonths(-3) && a.AppliedDate < lastQuarterEnd)
                .ToListAsync();
            var totalPrevQuarter = prevQuarterApps.Count;
            var successfulPrevQuarter = prevQuarterApps.Count(a => a.Status == ApplicationStatus.Accepted);
            var prevHireSuccessRate = totalPrevQuarter > 0 ? (double)successfulPrevQuarter / totalPrevQuarter * 100 : 0;
            var hireSuccessRateChange = hireSuccessRate - prevHireSuccessRate;

            return new AnalyticsEmployerDto
            {
                ApplicationRate = Math.Round(applicationRate, 1),
                ApplicationRateChange = Math.Round(applicationRateChange, 1),
                ProfileViews = profileViews,
                ProfileViewsChange = profileViewsChange,
                TimeToHire = timeToHire,
                HireSuccessRate = Math.Round(hireSuccessRate, 0),
                HireSuccessRateChange = Math.Round(hireSuccessRateChange, 0)
            };
        }


        ////////////////////////get hiring pipeline overview///////////////////////
        public async Task<HiringPipelineOverviewDto> GetHiringPipelineOverview(int employerId)
        {
            if (!await context.EmployerProfiles.AnyAsync(e => e.Id == employerId))
                throw new ArgumentException("Invalid employer ID", nameof(employerId));

            // 1. Total Applications
            var totalApplications = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.Job.IsApproved)
                .CountAsync();

            // 2. Under Review
            var underReview = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.Job.IsApproved && a.Status == ApplicationStatus.UnderReview)
                .CountAsync();

            // 3. Accepted
            var accepted = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.Job.IsApproved && a.Status == ApplicationStatus.Accepted)
                .CountAsync();

            // 4. Pending
            var pending = await context.Applications
                .Where(a => a.Job.EmployerId == employerId && a.Job.IsApproved && a.Status == ApplicationStatus.Pending)
                .CountAsync();

            return new HiringPipelineOverviewDto
            {
                TotalApplications = totalApplications,
                UnderReview = underReview,
                Accepted = accepted,
                Pending = pending
            };
        }


    }
}