using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ApplicationDto
{
	public class ApplicationSeedDto
	{
		public string FullName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string CurrentLocation { get; set; }
		public string CurrentJobTitle { get; set; }
		public string YearsOfExperience { get; set; }
		public string ResumeUrl { get; set; }
		public string? PortfolioUrl { get; set; }
		public string? LinkedInUrl { get; set; }
		public string? GitHubUrl { get; set; }
		public int ApplicantId { get; set; } // SeekerProfile Id
		public List<ApplicationDetailDto> Applications { get; set; } = new List<ApplicationDetailDto>();

	}

}
