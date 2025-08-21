using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.JobDto
{
    public class JobSeedDto
    {
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal? Salary { get; set; }
		public DateTime PostedDate { get; set; }
		public DateTime? ExpireDate { get; set; }
		public string WorkplaceType { get; set; } 
		public string JobType { get; set; } 
		public string? EducationLevel { get; set; }
		public string? ExperienceLevel { get; set; }
		public int? MinTeamSize { get; set; }
		public int? MaxTeamSize { get; set; }
		public bool IsActive { get; set; }
		public bool IsApproved { get; set; }
		public string? Requirements { get; set; }
		public string? Responsabilities { get; set; }
		public string? Benefits { get; set; }
		public int? EmployerId { get; set; }
		// Use IDs for seeding relationships
		public List<int> SkillIds { get; set; }
		public List<int> CategoryIds { get; set; }
	}
}
