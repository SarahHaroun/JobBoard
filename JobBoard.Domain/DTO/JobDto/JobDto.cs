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
		public DateTime PostedDate { get; set; }
		public DateTime? ExpireDate { get; set; }
		public string WorkplaceType { get; set; } // Enum as string
		public string JobType { get; set; }       // Enum as string
		public string? EducationLevel { get; set; } // Enum as string
		public string? ExperienceLevel { get; set; } // Enum as string
		public int? MinTeamSize { get; set; } 
		public int? MaxTeamSize { get; set; }
		public bool IsActive { get; set; }
		public string? Requirements { get; set; }
		public string? Responsabilities { get; set; }
		public string? Benefits { get; set; }
        public bool IsApproved { get; set; } = false;



        // Employer Info (summary)
        public string CompanyName { get; set; }
		public string CompanyLocation { get; set; }
		public string? CompanyImage { get; set; }
		public string? Website { get; set; }
		public string? Industry { get; set; }
		public string? CompanyDescription { get; set; }
		public string? CompanyMission { get; set; }
		public string? EmployeeRange { get; set; }
		public int? EstablishedYear { get; set; }

		// Lists
		public List<string> Categories { get; set; } = new();
		public List<string> Skills { get; set; } = new();
	}

}
