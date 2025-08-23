using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ApplicationDto
{
	public class EmployerApplicationListDto
	{
		public int Id { get; set; }
		public string ApplicantName { get; set; }
		public string JobTitle { get; set; } 
		public string CurrentPosition { get; set; } 
		public string AppliedDate { get; set; }
		public string Experience { get; set; }
		public ApplicationStatus Status { get; set; }
		public string StatusDisplay { get; set; }
		public string ResumeUrl { get; set; }
	}
}
