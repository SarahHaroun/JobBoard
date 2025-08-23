using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Repositories.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobBoard.Domain.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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

		public async Task<EmpProfileDto?> GetByUserId(string userId)
		{
			var emp = await context.EmployerProfiles.Include(e => e.User)
				.Include(e => e.PostedJobs).ThenInclude(js => js.Skills).FirstOrDefaultAsync(e => e.UserId == userId);
			if (emp == null)
				return null;

			return mapper.Map<EmpProfileDto>(emp);
		}
	}
}