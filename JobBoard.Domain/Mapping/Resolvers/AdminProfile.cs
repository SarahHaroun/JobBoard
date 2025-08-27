using AutoMapper;
using JobBoard.Domain.DTO.AdminDto;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping.Resolvers
{
    public class AdminProfile : Profile
    {
		public AdminProfile()
		{
			CreateMap<EmployerProfile, EmployerListDto>()
				.ForMember(dest => dest.Email, op => op.MapFrom(src => src.User.Email));


			CreateMap<SeekerProfile, SeekerListDto>()
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName))
				.ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skills.Select(s => s.SkillName)));
		}

	}
}
