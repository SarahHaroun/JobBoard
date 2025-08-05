using AutoMapper;
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
                .ForMember(dest => dest.CompanyImage, op => op.MapFrom(src => src.Employer.CompanyImage))
                .ForMember(dest => dest.Companylogo, op => op.MapFrom(src => src.Employer.Companylogo))
                .ForMember(dest => dest.CompanyDescription, op => op.MapFrom(src => src.Employer.CompanyDescription))
                .ForMember(dest => dest.Companymission, op => op.MapFrom(src => src.Employer.Companymission))
                .ForMember(dest => dest.EmployeesNumber, op => op.MapFrom(src => src.Employer.EmployeesNumber))
                .ForMember(dest => dest.EstablishedYear, op => op.MapFrom(src => src.Employer.EstablishedYear))
                .ForMember(dest => dest.Categories, op => op.MapFrom(src => src.Categories.Select(c => c.CategoryName).ToList()))
                .ForMember(dest => dest.Skills, op => op.MapFrom(src => src.Skills.Select(s => s.SkillName).ToList()))
                .ReverseMap();

			CreateMap<JobSeedDto, Job>()
				.ForMember(dest => dest.Skills, opt => opt.Ignore())
				.ForMember(dest => dest.Categories, opt => opt.Ignore())
				.ForMember(dest => dest.Id, opt => opt.Ignore()) // Don't map ID for new entities
				.ForMember(dest => dest.Employer, opt => opt.Ignore()) // Will be loaded by EF
				.ForMember(dest => dest.JobApplications, opt => opt.Ignore());


			CreateMap<CreateUpdateJobDto, Job>()
			   .ForMember(dest => dest.Id, opt => opt.Ignore())
			   .ForMember(dest => dest.EmployerId, opt => opt.Ignore())
			   .ForMember(dest => dest.Categories, opt => opt.Ignore())
			   .ForMember(dest => dest.Skills, opt => opt.Ignore())    
			   .ForMember(dest => dest.PostedDate, opt => opt.Ignore());

			CreateMap<JobSeedDto, Job>();


		}
	}
    
}
