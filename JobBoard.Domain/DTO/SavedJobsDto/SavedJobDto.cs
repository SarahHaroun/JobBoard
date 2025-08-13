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
		public string CompanyName { get; set; }
		public DateTime SavedAt { get; set; } = DateTime.UtcNow;
	}
}
