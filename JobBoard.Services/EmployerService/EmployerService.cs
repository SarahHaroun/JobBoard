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

namespace JobBoard.Services.EmployerService
{
    public class EmployerService : IEmployerService
    {
        private readonly ApplicationDbContext context;

        public EmployerService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<string> Create(CreateEmpProfileDto model)
        {
            //check if employer exist
            var existUser = await context.EmployerProfiles.FirstOrDefaultAsync(u => u.UserId == model.UserId);
            if (existUser != null)
                return "Employer profile already exists";

            //if the employer not exist before, create new profile
            var newEmployer = new EmployerProfile();
            newEmployer.CompanyName = model.CompanyName;
            newEmployer.CompanyLocation = model.CompanyLocation;
            newEmployer.UserId = model.UserId;

            context.EmployerProfiles.Add(newEmployer);
            await context.SaveChangesAsync();

            return "Employer created successfully";

        }

        public async Task<bool> DeleteById(int id)
        {
            var employer = await context.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == id);
            if (employer == null)
            {
                return false;
            }
            context.EmployerProfiles.Remove(employer);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmpProfileDto>> GetAll()
        {
            var employers = await context.EmployerProfiles.ToListAsync();

            var empDtoList = new List<EmpProfileDto>();

            foreach (var emp in employers)
            {
                empDtoList.Add(new EmpProfileDto
                {
                    Id = emp.Id,
                    CompanyName = emp.CompanyName,
                    CompanyLocation = emp.CompanyLocation,
                    UserId = emp.UserId
                });
            }
            return empDtoList;
        }

        public async Task<EmpProfileDto> GetById(int id)
        {
            var employer = await context.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == id);
            if (employer == null)
                return null;

            return new EmpProfileDto
            {
                Id = employer.Id,
                CompanyName = employer.CompanyName,
                CompanyLocation = employer.CompanyLocation,
                UserId = employer.UserId
            };
        }

        public async Task<bool> Update(int id, UpdateEmpProfileDto empProfile)
        {
            var employer = await context.EmployerProfiles.FirstOrDefaultAsync(e => e.Id == id);
            if (employer == null)
                return false;

            employer.CompanyName = empProfile.CompanyName;
            context.EmployerProfiles.Update(employer);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<EmpProfileDto?> GetByUserIdAsync(string userId)
        {
            var emp = await context.EmployerProfiles.FirstOrDefaultAsync(e => e.UserId == userId);
            if (emp == null) return null;

            return new EmpProfileDto
            {
                Id = emp.Id,
                CompanyName = emp.CompanyName,
                CompanyLocation = emp.CompanyLocation,
                UserId = emp.UserId
            };
        }

    }
}
