using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SavedJobsDto
{
    public class SavedJobDto
    {
		public int Id { get; set; }
		public int JobId { get; set; }
		public string JobTitle { get; set; }
		public string Description { get; set; }
		public string CompanyName { get; set; }
		public string Location { get; set; } //From Employer
		public decimal? Salary { get; set; }
		public string WorkplaceType { get; set; }
		public string JobType { get; set; }
		public DateTime SavedAt { get; set; } = DateTime.UtcNow;
	}
}
