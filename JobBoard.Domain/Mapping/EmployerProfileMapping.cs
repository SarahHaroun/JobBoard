using AutoMapper;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping
{
    public class EmployerProfileMapping : Profile
    {
        public EmployerProfileMapping()
        {
            CreateMap<EmployerProfile, EmpProfileDto>()
                .ForMember(dest=> dest.Email , op=> op.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, op => op.MapFrom(src => src.User.PhoneNumber))
                .ReverseMap();

            CreateMap<EmpProfileUpdateDto, EmployerProfile>();


        }
    }
}
