using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.JobDto
{
	public class JobDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal? Salary { get; set; }
		public string WorkplaceType { get; set; } // Enum as string
		public string JobType { get; set; }       // Enum as string
		public DateTime PostedDate { get; set; }
		public DateTime? ExpireDate { get; set; }
		public string? Requirements { get; set; }
		public int? MinTeamSize { get; set; } //New
		public int? MaxTeamSize { get; set; }
		public string? EducationLevel { get; set; } // Enum as string
		public string? ExperienceLevel { get; set; } // Enum as string
		public bool IsActive { get; set; }

		// Employer Info (summary)
		public string CompanyName { get; set; }
		public string CompanyLocation { get; set; }
		public string Website { get; set; }

		// Lists
		public List<string> Categories { get; set; } = new();
		public List<string> Skills { get; set; } = new();
	}

}
