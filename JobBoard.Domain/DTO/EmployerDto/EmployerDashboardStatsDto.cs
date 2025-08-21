using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.EmployerDto
{
    public class EmployerDashboardStatsDto
    {
		public int TotalActiveJobs { get; set; }
		public int ApplicationsThisMonth { get; set; }
		public int JobsExpiringSoon { get; set; }

	}
}
