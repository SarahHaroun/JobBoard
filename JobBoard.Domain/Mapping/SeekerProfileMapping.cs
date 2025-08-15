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
            // -----------------------------
            // Seeker Profile -> DTO
            // -----------------------------
            CreateMap<SeekerProfile, SeekerProfileDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skills.Select(s => s.SkillName)))
                .ForMember(dest => dest.InterestName, opt => opt.MapFrom(src => src.seekerInterests.Select(i => i.InterestName)))
                .ForMember(dest => dest.CertificateName, opt => opt.MapFrom(src => src.seekerCertificates.Select(c => c.CertificateName)))
                .ForMember(dest => dest.TrainingName, opt => opt.MapFrom(src => src.SeekerTraining.Select(t => t.TrainingName)))
                .ForMember(dest => dest.SeekerEducations, opt => opt.MapFrom(src => src.SeekerEducations))
                .ForMember(dest => dest.SeekerExperiences, opt => opt.MapFrom(src => src.SeekerExperiences));

            // -----------------------------
            // DTO -> Seeker Profile
            // -----------------------------
            CreateMap<SeekerProfileUpdateDto, SeekerProfile>()
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills.Select(s => new Skill { SkillName = s })))
                .ForMember(dest => dest.seekerInterests, opt => opt.MapFrom(src => src.Interests.Select(i => new SeekerInterest { InterestName = i })))
                .ForMember(dest => dest.seekerCertificates, opt => opt.MapFrom(src => src.Certificates.Select(c => new SeekerCertificate { CertificateName = c })))
                .ForMember(dest => dest.SeekerTraining, opt => opt.MapFrom(src => src.Trainings.Select(t => new SeekerTraining { TrainingName = t })))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // -----------------------------
            // Education
            // -----------------------------
            CreateMap<SeekerEducation, SeekerEducationDto>().ReverseMap();
            CreateMap<SeekerEducationUpdateDto, SeekerEducation>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // -----------------------------
            // Experience
            // -----------------------------
            CreateMap<SeekerExperience, SeekerExperienceDto>().ReverseMap();
            CreateMap<SeekerExperienceUpdateDto, SeekerExperience>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

			// -----------------------------
			// UploadingFiles
			// -----------------------------
			CreateMap<SeekerProfileUpdateDto, SeekerProfile>()
            .ForMember(dest => dest.CV_Url, opt => opt.Ignore()) 
            .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore());

		}
    }
}
