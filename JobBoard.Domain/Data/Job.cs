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
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public decimal? Salary { get; set; }
        public WorkplaceType WorkplaceType { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpireDate { get; set; }

		public string Location { get; set; } // New: Ex: Cairo, Egypt 

		public int NumberOfPositions { get; set; } //New
		public string CareerLevel { get; set; } //New
		public string EducationLevel { get; set; } //New
		public string? Requirements { get; set; } //New

		/*------------------------Category--------------------------*/

		[ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        /*------------------------Applications--------------------------*/
        public List<Application>? JobApplications { get; set; }

        /*------------------------Employer--------------------------*/
        [ForeignKey("EmployerProfile")]
        public int EmployerId { get; set; }
        public EmployerProfile Employer { get; set; }

        /*------------------------Skills--------------------------*/
        public List<Skill>? Skills { get; set; }
    }
}
