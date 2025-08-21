using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.DashboardSpecifications
{
    public class ApplicationsMonthSpecification : BaseSpecifications<Application>
    {
		public ApplicationsMonthSpecification(int employerId, DateTime startOfMonth)
			: base(a => a.Job.EmployerId == employerId &&a.AppliedDate >= startOfMonth)
		{
			AddIncludes(a => a.Job);
		}
	}
}
