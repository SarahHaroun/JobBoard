using AutoMapper;
using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO.JobDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.JobsDto;
using JobBoard.Domain.Shared;

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
                .ForMember(dest => dest.CompanyImage, op => op.MapFrom(src => src.Employer.CompanyImage))
                .ForMember(dest => dest.Website, op => op.MapFrom(src => src.Employer.Website))
                .ForMember(dest => dest.Industry, op => op.MapFrom(src => src.Employer.Industry))
                .ForMember(dest => dest.CompanyDescription, op => op.MapFrom(src => src.Employer.CompanyDescription))
                .ForMember(dest => dest.CompanyMission, op => op.MapFrom(src => src.Employer.CompanyMission))
                .ForMember(dest => dest.EmployeeRange, op => op.MapFrom(src => src.Employer.EmployeeRange))
                .ForMember(dest => dest.EstablishedYear, op => op.MapFrom(src => src.Employer.EstablishedYear))
                .ForMember(dest => dest.Locaton, op => op.MapFrom(src => src.Employer.CompanyLocation))
                .ForMember(dest => dest.Email, op => op.MapFrom(src => src.Employer.User.Email))
                .ForMember(dest => dest.Phone, op => op.MapFrom(src => src.Employer.User.PhoneNumber))
                .ForMember(dest => dest.Categories, op => op.MapFrom(src => src.Categories.Select(c => c.CategoryName).ToList()))
                .ForMember(dest => dest.Skills, op => op.MapFrom(src => src.Skills.Select(s => s.SkillName).ToList()))
                .ReverseMap();

		CreateMap<JobSeedDto, Job>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.EmployerId, opt => opt.Ignore()) // Handle manually
               .ForMember(dest => dest.Employer, opt => opt.Ignore())
               .ForMember(dest => dest.Skills, opt => opt.Ignore()) // Handle manually
               .ForMember(dest => dest.Categories, opt => opt.Ignore()) // Handle manually
               .ForMember(dest => dest.JobApplications, opt => opt.Ignore());


            CreateMap<CreateUpdateJobDto, Job>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.EmployerId, opt => opt.Ignore())
               .ForMember(dest => dest.Categories, opt => opt.Ignore())
               .ForMember(dest => dest.Skills, opt => opt.Ignore());    
			   //.ForMember(dest => dest.PostedDate, opt => opt.Ignore());

			CreateMap<JobSeedDto, Job>();

            CreateMap<Job, TopPerformingJobDto>()
                .ForMember(dest => dest.ApplicationsCount, opt => opt.MapFrom(src => src.JobApplications != null ? src.JobApplications.Count : 0));

			CreateMap<Job, RecentJobDto>()
	            .ForMember(dest => dest.ApplicationsCount,
		            opt => opt.MapFrom(src => src.JobApplications != null ? src.JobApplications.Count : 0))
	            .ForMember(dest => dest.PostedAgo,
		            opt => opt.MapFrom(src => DateTimeHelper.CalculateTimeAgo(src.PostedDate)));

            CreateMap<Job, PostedJobsDto>()
                .ForMember(dest => dest.ApplicationsCount, opt => opt.MapFrom(src => src.JobApplications != null ? src.JobApplications.Count : 0));
		}
	}
    
}
