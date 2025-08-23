using JobBoard.Domain.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ApplicationDto
{
	public class CreateApplicationDto
	{
		[Required]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string CurrentLocation { get; set; }

		[Required]
		public string CurrentJobTitle { get; set; }

		[Required]
		public string YearsOfExperience { get; set; }

		public bool RemoveResume { get; set; } = false;

		[Required]
		[AllowedExtensions("pdf", "doc", "docx", ErrorMessage = "Only PDF and Word documents are allowed for CV.")]
		public IFormFile ResumeUrl { get; set; }
		public string? CoverLetter { get; set; }
		public string? PortfolioUrl { get; set; }
		public string? LinkedInUrl { get; set; }
		public string? GitHubUrl { get; set; }

		[Required]
		public int JobId { get; set; }
	}
}