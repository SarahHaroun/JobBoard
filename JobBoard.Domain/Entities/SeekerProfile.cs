using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class SeekerProfile 
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Title { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address {  get; set; }
        public string? CV_Url { get; set; }
        public Gender Gender { get; set; }
        public string? Summary { get; set; }
        public string? ProfileImageUrl { get; set; }
          


        /*------------------------user--------------------------*/
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        /*------------------------Application--------------------------*/
        public List<Application>? UserApplications { get; set; }

		/*------------------------Skills--------------------------*/
		public ICollection<Skill>? Skills { get; set; } = new List<Skill>();

        /*------------------------SeekerEducation--------------------------*/
        public ICollection<SeekerEducation>? SeekerEducations { get; set; } = new List<SeekerEducation>();

        /*------------------------SeekerExperience--------------------------*/
        public ICollection<SeekerExperience>? SeekerExperiences { get; set; } = new List<SeekerExperience>();


        /*------------------------SeekerTraining--------------------------*/
        public ICollection<SeekerTraining>? SeekerTraining { get; set; } = new List<SeekerTraining>();


        /*------------------------SeekerCertificate--------------------------*/
        public ICollection<SeekerCertificate>? seekerCertificates { get; set; } = new List<SeekerCertificate>();


        /*------------------------SeekerInterest--------------------------*/
        public ICollection<SeekerInterest>? seekerInterests { get; set; } = new List<SeekerInterest>();

    }
}
