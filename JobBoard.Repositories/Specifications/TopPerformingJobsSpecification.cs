using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
	public class TopPerformingJobsSpecification : BaseSpecifications<Job>
	{
		public TopPerformingJobsSpecification(int employerId, int limit = 5)
			: base(j => j.EmployerId == employerId &&
					   j.IsApproved &&
					   j.JobApplications.Any())
		{
			AddIncludes(j => j.JobApplications);
			AddOrderByDesc(j => j.JobApplications.Count);
			AddPagination(0, limit);
		}
	}
}
