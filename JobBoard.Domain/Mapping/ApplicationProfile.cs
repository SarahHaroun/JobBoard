using AutoMapper;
using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Mapping.Resolvers;
using JobBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping
{
	public class ApplicationProfile : Profile
	{
		public ApplicationProfile()
		{
			CreateMap<CreateApplicationDto, Application>()
			.ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
			.ForMember(dest => dest.Status, opt => opt.MapFrom(src => ApplicationStatus.Pending))
			.ForMember(dest => dest.ResumeUrl, opt => opt.Ignore());

			CreateMap<Application, ApplicationDto>()
				.ForMember(dest => dest.Job, opt => opt.MapFrom(src => src.Job));

			CreateMap<Job, JobSummaryDto>()
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Employer != null ? src.Employer.CompanyName : string.Empty))
				.ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom(src => src.Employer.CompanyLocation))
				.ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary));

			CreateMap<SeekerProfile, ApplicantSummaryDto>();


			CreateMap<(ApplicationSeedDto userDto, ApplicationDetailDto appDto), Application>()
			   .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.userDto.FullName))
			   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.userDto.Email))
			   .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.userDto.PhoneNumber))
			   .ForMember(dest => dest.CurrentLocation, opt => opt.MapFrom(src => src.userDto.CurrentLocation))
			   .ForMember(dest => dest.CurrentJobTitle, opt => opt.MapFrom(src => src.userDto.CurrentJobTitle))
			   .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => src.userDto.YearsOfExperience))
			   .ForMember(dest => dest.ResumeUrl, opt => opt.MapFrom<ApplicationUrlResolver>())
			   .ForMember(dest => dest.PortfolioUrl, opt => opt.MapFrom(src => src.userDto.PortfolioUrl))
			   .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.userDto.LinkedInUrl))
			   .ForMember(dest => dest.GitHubUrl, opt => opt.MapFrom(src => src.userDto.GitHubUrl))
			   .ForMember(dest => dest.CoverLetter, opt => opt.MapFrom(src => src.appDto.CoverLetter))
			   .ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(src => src.appDto.AppliedDate))
			   .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.appDto.Status))
			   .ForMember(dest => dest.ApplicantId, opt => opt.MapFrom(src => src.userDto.ApplicantId))
			   .ForMember(dest => dest.JobId, opt => opt.MapFrom(src => src.appDto.JobId))
			   .ForMember(dest => dest.Id, opt => opt.Ignore())
			   .ForMember(dest => dest.Job, opt => opt.Ignore())
			   .ForMember(dest => dest.Applicant, opt => opt.Ignore());

			CreateMap<Application, EmployerApplicationListDto>()
		   .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.FullName))
		   .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
		   .ForMember(dest => dest.CurrentPosition, opt => opt.MapFrom(src => src.CurrentJobTitle))
		   .ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(src => DateTimeHelper.CalculateTimeAgo(src.AppliedDate)))
		   .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => $"{src.YearsOfExperience} years experience"))
		   .ForMember(dest => dest.StatusDisplay, opt => opt.MapFrom(src => ApplicationStatusHelper.GetStatusDisplay(src.Status)));

		}
	}
}