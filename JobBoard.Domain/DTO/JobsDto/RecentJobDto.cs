using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.JobsDto
{
    public class RecentJobDto
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime PostedDate { get; set; }
		public int ApplicationsCount { get; set; }
		public bool IsActive { get; set; }
		public string Status => IsActive ? "Active" : "Filled";
		public string PostedAgo { get; set; }
	}
}
