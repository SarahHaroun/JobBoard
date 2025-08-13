using AutoMapper;
using JobBoard.Domain.DTO.SavedJobsDto;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping.Resolvers
{
    public class SavedJobProfile : Profile
    {
		public SavedJobProfile()
		{
			CreateMap<SavedJob, SavedJobDto>()
				.ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Job.Employer.CompanyName));

			CreateMap<CreateSavedJobDto, SavedJob>();
		}
	}
}
