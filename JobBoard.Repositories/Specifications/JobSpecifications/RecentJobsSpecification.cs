using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.JobSpecifications
{
    public class RecentJobsSpecification : BaseSpecifications<Job>
    {
		public RecentJobsSpecification(int employerId, int limit = 3)
		: base(j => j.EmployerId == employerId && j.IsApproved)
		{
			AddIncludes(j => j.JobApplications);
			AddOrderByDesc(j => j.PostedDate); 
		}
	}
}
