using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.JobSpecifications
{
	public class JobUpdateSpecification : BaseSpecifications<Job>
	{
		public JobUpdateSpecification(int id, int employerId)
			: base(j => j.Id == id && j.EmployerId == employerId) // without IsApproved
		{
			AddIncludes(j => j.Skills);
			AddIncludes(j => j.Categories);
			AddIncludes(j => j.Employer);
		}
	}
}
