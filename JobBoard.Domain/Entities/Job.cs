using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Salary { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public WorkplaceType WorkplaceType { get; set; }
        public JobType JobType { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public ExperienceLevel? ExperienceLevel { get; set; } 
        public int? MinTeamSize { get; set; } 
        public int? MaxTeamSize { get; set; } 
        public bool IsActive { get; set; } = true;
        public string? Requirements { get; set; } 
		public string? Responsabilities { get; set; }
		public string? Benefits { get; set; }


		/*------------------------Category--------------------------*/
		public ICollection<Category>? Categories { get; set; }


        /*------------------------Applications--------------------------*/
        public ICollection<Application>? JobApplications { get; set; }

        /*------------------------Employer--------------------------*/
        [ForeignKey("UserProfile")]
        public int EmployerId { get; set; }
        public EmployerProfile Employer { get; set; }

        /*------------------------Skills--------------------------*/
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}