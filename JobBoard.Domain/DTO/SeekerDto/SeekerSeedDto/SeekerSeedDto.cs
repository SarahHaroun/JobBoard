using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto.SeekerSeedDto
{
    public class SeekerSeedDto
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Title { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? CV_Url { get; set; }
        public Gender? Gender { get; set; }
        public string? Summary { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? UserEmail { get; set; }



        // Navigation collections
        public List<int>? Skills { get; set; }

        public List<SeekerCertificateSeedDto>? seekerCertificates { get; set; }
        public List<SeekerTrainingSeedDto>? SeekerTraining { get; set; }
        public List<SeekerInterstsSeedDto>? seekerInterests { get; set; }


        public List<SeekerExperienceSeedDto>? SeekerExperiences { get; set; } 
        public List<SeekerEducationSeedDto>? SeekerEducations { get; set; } 

    }
}
