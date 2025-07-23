using JobBoard.Domain.Data;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Repositories.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services.SeekerService
{
    public class SeekerService : ISeekerService
    {
        private readonly ApplicationDbContext context;
        public SeekerService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<SeekerProfileResultDto> Create(SeekerProfileDto seekerProfile)
        {
            //check if seeker profile already exists
            var existingProfile = await context.SeekerProfiles.FirstOrDefaultAsync(s => s.UserId == seekerProfile.UserId);
            if (existingProfile != null)
                return new SeekerProfileResultDto
                {
                    Success = false,
                    Message = "Seeker profile already exists."
                };

            //if not, create a new profile
            var newSeekerProfile = new SeekerProfile()
            {
                FirstName = seekerProfile.FirstName,
                LastName = seekerProfile.LastName,
                Address = seekerProfile.Address,
                CV_Url = seekerProfile.CV_Url,
                Experience_Level = seekerProfile.Experience_Level,
                Gender = seekerProfile.Gender,
                UserId = seekerProfile.UserId,

            };
            await context.SeekerProfiles.AddAsync(newSeekerProfile);
            await context.SaveChangesAsync();

            //var dto = new SeekerProfileDto
            //{
            //    Id = newSeekerProfile.Id,
            //    FirstName = newSeekerProfile.FirstName,
            //    LastName = newSeekerProfile.LastName,
            //    Address = newSeekerProfile.Address,
            //    CV_Url = newSeekerProfile.CV_Url,
            //    Experience_Level = newSeekerProfile.Experience_Level,
            //    Gender = newSeekerProfile.Gender,
            //    UserId = newSeekerProfile.UserId
            //};


            return new SeekerProfileResultDto
            {
                Success = true,
                Message = "Seeker profile created successfully.",
                Data = MapToDto(newSeekerProfile)
            };


        }

        public async Task<bool> DeleteById(int id)
        {
            var seekerProfile = await context.SeekerProfiles.FirstOrDefaultAsync(s => s.Id == id);
            if (seekerProfile == null)
            {
                return false;
            }
            context.SeekerProfiles.Remove(seekerProfile);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SeekerProfileDto>> GetAll()
        {
            var seekers = await context.SeekerProfiles.ToListAsync();
            var SeekerDtoList = new List<SeekerProfileDto>();

            foreach (var seeker in seekers)
            {
                //SeekerDtoList.Add(new SeekerProfileDto
                //{
                //    Id = seeker.Id,
                //    FirstName = seeker.FirstName,
                //    LastName = seeker.LastName,
                //    Address = seeker.Address,
                //    CV_Url = seeker.CV_Url,
                //    Experience_Level = seeker.Experience_Level,
                //    Gender = seeker.Gender,
                //    UserId = seeker.UserId
                //});
                SeekerDtoList.Add(MapToDto(seeker));
            }

            return SeekerDtoList;

        }

        public async Task<SeekerProfileDto> GetByUserId(string userId)
        {
            var seekerProfile = await context.SeekerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (seekerProfile == null)
            {
                return null;
            }
            return MapToDto(seekerProfile);
        }


        public async Task<SeekerProfileResultDto> Update(int id, SeekerProfileDto seekerProfile)
        {
            var existingProfile = await context.SeekerProfiles.FirstOrDefaultAsync(s => s.Id == id);
            if (existingProfile == null)
            {
                return new SeekerProfileResultDto
                {
                    Success = false,
                    Message = "Seeker profile not found."
                };
            }
            // Update the existing profile with new values
            existingProfile.FirstName = seekerProfile.FirstName;
            existingProfile.LastName = seekerProfile.LastName;
            existingProfile.Address = seekerProfile.Address;
            existingProfile.CV_Url = seekerProfile.CV_Url;
            existingProfile.Experience_Level = seekerProfile.Experience_Level;
            existingProfile.Gender = seekerProfile.Gender;

            context.SeekerProfiles.Update(existingProfile);
            await context.SaveChangesAsync();
            //var updatedDto = new SeekerProfileDto
            //{
            //    Id = existingProfile.Id,
            //    FirstName = existingProfile.FirstName,
            //    LastName = existingProfile.LastName,
            //    Address = existingProfile.Address,
            //    CV_Url = existingProfile.CV_Url,
            //    Experience_Level = existingProfile.Experience_Level,
            //    Gender = existingProfile.Gender,
            //};
            return new SeekerProfileResultDto
            {
                Success = true,
                Message = "Seeker profile updated successfully.",
                Data = MapToDto(existingProfile)
            };
        }

        private SeekerProfileDto MapToDto(SeekerProfile seeker)
        {
            return new SeekerProfileDto
            {
                Id = seeker.Id,
                FirstName = seeker.FirstName,
                LastName = seeker.LastName,
                Address = seeker.Address,
                CV_Url = seeker.CV_Url,
                Experience_Level = seeker.Experience_Level,
            };
        }

    } 
}