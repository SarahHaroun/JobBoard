using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto
{
    public class SeekerProfileUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; } 
        public string? Summary { get; set; }
        public string? CV_Url { get; set; }
        public string? ProfileImageUrl { get; set; }

        public List<string>? Skills { get; set; }

        // Interests
        public List<string>? Interests { get; set; }

        // Certificates
        public List<string>? Certificates { get; set; }

        // Trainings
        public List<string>? Trainings { get; set; }

        // Educations
        public List<SeekerEducationDto>? SeekerEducations { get; set; }

        // Experiences
        public List<SeekerExperienceDto>? SeekerExperiences { get; set; }


    }
}
