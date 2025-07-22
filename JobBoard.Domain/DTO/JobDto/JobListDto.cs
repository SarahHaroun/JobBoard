using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.JobDto
{
    public class JobListDto
    {
		public int Id { get; set; }
		public string Title { get; set; }

		// Employer Info (summary)
		public string CompanyName { get; set; } // From Employer
		public string Location { get; set; } //From Employer
		public decimal? Salary { get; set; }
		public string WorkplaceType { get; set; } // Enum
		public string JobType { get; set; } // Enum
		public DateTime PostedDate { get; set; }
		// List
		public List<string> Skills { get; set; } 
	}
}
