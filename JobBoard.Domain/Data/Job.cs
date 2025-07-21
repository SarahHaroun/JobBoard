using JobBoard.Domain.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Data
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Salary { get; set; }
        public WorkplaceType WorkplaceType { get; set; }
        public JobType JobType { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
		public EducationLevel? EducationLevel { get; set; } 
		public int? MinTeamSize { get; set; } //New
		public int? MaxTeamSize { get; set; } //New
		public ExperienceLevel? ExperienceLevel { get; set; } //New
		public string? Requirements { get; set; } //New
		public bool IsActive { get; set; } = true; //New

        /*------------------------Category--------------------------*/
        public List<Category>? Categories { get; set; }


        /*------------------------Applications--------------------------*/
        public List<Application>? JobApplications { get; set; }

        /*------------------------Employer--------------------------*/
        [ForeignKey("EmployerProfile")]
        public int EmployerId { get; set; }
        public EmployerProfile Employer { get; set; }

        /*------------------------Skills--------------------------*/
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
	}
}
