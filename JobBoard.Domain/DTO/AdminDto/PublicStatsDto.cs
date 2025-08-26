using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AdminDto
{
	public class PublicStatsDto
	{
		public int TotalSeekers { get; set; }
		public int TotalEmployers { get; set; }
		public int TotalJobs { get; set; }
		public int ApprovedJobs { get; set; }
		public int ActiveJobs { get; set; }
	}
}
