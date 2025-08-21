using AutoMapper;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.DTO.SeekerDto.SeekerSeedDto;
using JobBoard.Domain.DTO.UserDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Mapping.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping
{
    public class UserProfileMapping :Profile
    {
        public UserProfileMapping()
        {
            // DTO to Entity mappings
            CreateMap<UserSeedDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            CreateMap<EmployerSeedDto, EmployerProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyImage, opt => opt.MapFrom<CompanyImageUrlResolver>());



        }
    }
}
