using AutoMapper;
using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
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
		}

	}
}
