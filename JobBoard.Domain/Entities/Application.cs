using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class Application
    {
        public int Id { get; set; }

		//Applicant info
		public string FullName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string CurrentLocation { get; set; }
		public string CurrentJobTitle { get; set; }
		public string YearsOfExperience { get; set; }

		//Documents
		public string ResumeUrl { get; set; }
		public string? CoverLetter { get; set; }
		//Links
		public string? PortfolioUrl { get; set; }
		public string? LinkedInUrl { get; set; }
		public string? GitHubUrl { get; set; }

		public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
		/*------------------------job--------------------------*/
        public int JobId { get; set; }
        public Job Job { get; set; }

        /*------------------------SeekerProfile--------------------------*/

        [ForeignKey("SeekerProfile")]
        public int ApplicantId { get; set; } // refers to seeker
        public SeekerProfile Applicant { get; set; }
    }
}
