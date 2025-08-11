using AutoMapper;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
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
        private readonly IMapper mapper;
        public SeekerService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //public async Task<SeekerProfileResultDto> Create(SeekerProfileDto seekerProfile)
        //{
        //    //check if seeker profile already exists
        //    var existingProfile = await context.SeekerProfiles.FirstOrDefaultAsync(s => s.UserId == seekerProfile.UserId);
        //    if (existingProfile != null)
        //        return new SeekerProfileResultDto
        //        {
        //            Success = false,
        //            Message = "Seeker profile already exists."
        //        };

        //    //if not, create a new profile
        //    var newSeekerProfile = new SeekerProfile()
        //    {
        //        FirstName = seekerProfile.FirstName,
        //        LastName = seekerProfile.LastName,
        //        Address = seekerProfile.Address,
        //        CV_Url = seekerProfile.CV_Url,
        //        UserId = seekerProfile.UserId,

        //    };
        //    await context.SeekerProfiles.AddAsync(newSeekerProfile);
        //    await context.SaveChangesAsync();

        //    //var dto = new SeekerProfileDto
        //    //{
        //    //    Id = newSeekerProfile.Id,
        //    //    FirstName = newSeekerProfile.FirstName,
        //    //    LastName = newSeekerProfile.LastName,
        //    //    Address = newSeekerProfile.Address,
        //    //    CV_Url = newSeekerProfile.CV_Url,
        //    //    Experience_Level = newSeekerProfile.Experience_Level,
        //    //    Gender = newSeekerProfile.Gender,
        //    //    UserId = newSeekerProfile.UserId
        //    //};


        //    return new SeekerProfileResultDto
        //    {
        //        Success = true,
        //        Message = "Seeker profile created successfully.",
        //        Data = MapToDto(newSeekerProfile)
        //    };


        //}

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
            //var SeekerDtoList = new List<SeekerProfileDto>();
            //foreach (var seeker in seekers)
            //{                
            //    SeekerDtoList.Add(MapToDto(seeker));
            //}
            //return SeekerDtoList;

            return mapper.Map<List<SeekerProfileDto>>(seekers);

        }

        public async Task<SeekerProfileDto> GetByUserId(string userId)
        {
            var seeker = await context.SeekerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (seeker == null)
            {
                return null;
            }
            return mapper.Map<SeekerProfileDto>(seeker);
        }


        public async Task<bool> Update(int id, SeekerProfileUpdateDto seekerProfile)
        {
            var seeker = await context.SeekerProfiles.FirstOrDefaultAsync(s => s.Id == id);
            if (seeker == null)
            {
                return false;
            }
            // Update the properties of the seeker profile
            mapper.Map(seekerProfile, seeker);

            context.SeekerProfiles.Update(seeker);  
            await context.SaveChangesAsync();
            return true;

        }

       

        //private SeekerProfileDto MapToDto(SeekerProfile seeker)
        //{
        //    return new SeekerProfileDto
        //    {
        //        Id = seeker.Id,
        //        FirstName = seeker.FirstName,
        //        LastName = seeker.LastName,
        //        Address = seeker.Address,
        //        CV_Url = seeker.CV_Url,
        //        UserId= seeker.UserId,
        //    };
        //}

    } 
}