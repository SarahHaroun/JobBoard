using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AdminDto
{
	public class GrowthStatsDto
	{
		public double JobsGrowth { get; set; }
		public double ApprovalGrowth { get; set; }
		public double ActiveJobsGrowth { get; set; }
	}
}
