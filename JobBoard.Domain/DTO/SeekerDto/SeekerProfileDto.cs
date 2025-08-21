using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto
{
    public class SeekerProfileDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Title { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? CV_Url { get; set; }
        public Gender? Gender { get; set; }
        public string? Summary { get; set; }
        public string? ProfileImageUrl { get; set; }


        // Navigation collections
        public List<string>? CertificateName { get; set; } = new();
        public List<string>? TrainingName { get; set; } = new();
        public List<string>? InterestName { get; set; } = new();
        public List<string>? SkillName { get; set; } = new();

        public List<SeekerExperienceDto>? SeekerExperiences { get; set; } = new();
        public List<SeekerEducationDto>? SeekerEducations { get; set; } = new();






    }
}
