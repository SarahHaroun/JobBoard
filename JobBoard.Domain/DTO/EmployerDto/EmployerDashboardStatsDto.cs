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

    public class AnalyticsEmployerDto
    {
        public double ApplicationRate { get; set; }
        public double ApplicationRateChange { get; set; }
        public int ProfileViews { get; set; }
        public int ProfileViewsChange { get; set; }
        public int TimeToHire { get; set; }
        public double HireSuccessRate { get; set; }
        public double HireSuccessRateChange { get; set; }
    }

    public class HiringPipelineOverviewDto
    {
        public int TotalApplications { get; set; }
        public int UnderReview { get; set; }
        public int Accepted { get; set; }
        public int Pending { get; set; }
    }
}
