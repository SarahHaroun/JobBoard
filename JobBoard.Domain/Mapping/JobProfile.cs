﻿using AutoMapper;
using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO.JobDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping
{
    public class JobProfile : Profile
    {
        public JobProfile() {
            CreateMap<Job, JobListDto>()
				.ForMember(dest => dest.CompanyName, op => op.MapFrom(src => src.Employer.CompanyName))
				.ForMember(dest => dest.Location, op => op.MapFrom(src => src.Employer.CompanyLocation))
                .ForMember(dest => dest.Skills, op => op.MapFrom(src => src.Skills.Select(s => s.SkillName).ToList()));

            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.CompanyName, op => op.MapFrom(src => src.Employer.CompanyName))
                .ForMember(dest => dest.CompanyLocation, op => op.MapFrom(src => src.Employer.CompanyLocation))
                .ForMember(dest => dest.Website, op => op.MapFrom(src => src.Employer.Website))
                .ForMember(dest => dest.Categories, op => op.MapFrom(src => src.Categories.Select(c => c.CategoryName).ToList()))
                .ForMember(dest => dest.Skills, op => op.MapFrom(src => src.Skills.Select(s => s.SkillName).ToList()))
                .ReverseMap();


            CreateMap<Job, JobDetailsDto>()
            .ForMember(dest => dest.CompanyName, op => op.MapFrom(src => src.Employer.CompanyName))
            .ForMember(dest => dest.CompanyLocation, op => op.MapFrom(src => src.Employer.CompanyLocation))
            .ForMember(dest => dest.Website, op => op.MapFrom(src => src.Employer.Website))
            .ForMember(dest => dest.WorkplaceType, op => op.MapFrom(src => src.WorkplaceType.ToString()))
            .ForMember(dest => dest.JobType, op => op.MapFrom(src => src.JobType.ToString()))
            .ForMember(dest => dest.EducationLevel, op => op.MapFrom(src => src.EducationLevel.HasValue ? src.EducationLevel.ToString() : null))
            .ForMember(dest => dest.ExperienceLevel, op => op.MapFrom(src => src.ExperienceLevel.HasValue ? src.ExperienceLevel.ToString() : null))
            .ForMember(dest => dest.Categories, op => op.MapFrom(src => src.Categories.Select(c => c.CategoryName).ToList()))
            .ForMember(dest => dest.Skills, op => op.MapFrom(src => src.Skills.Select(s => s.SkillName).ToList()));


            CreateMap<string, Category>()
           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src));

            CreateMap<string, Skill>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src));
        }
    }
}
