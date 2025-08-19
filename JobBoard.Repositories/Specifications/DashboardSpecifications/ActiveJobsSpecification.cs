using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.DashboardSpecifications
{
    public class ActiveJobsSpecification : BaseSpecifications<Job>
    {
		public ActiveJobsSpecification(int employerId)
			: base(j => j.EmployerId == employerId &&
					   j.IsApproved &&
					   j.IsActive &&
					   (!j.ExpireDate.HasValue || j.ExpireDate > DateTime.Now))
		{
		}
	}
}
