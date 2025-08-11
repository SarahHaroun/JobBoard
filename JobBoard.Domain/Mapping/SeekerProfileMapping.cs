using AutoMapper;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping
{
    public class SeekerProfileMapping : Profile
    {
        public SeekerProfileMapping()
        {
            CreateMap<SeekerProfile, SeekerProfileDto>();
                
        }
    }
}
