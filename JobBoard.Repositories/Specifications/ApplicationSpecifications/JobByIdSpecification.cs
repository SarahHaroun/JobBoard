using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.ApplicationSpecifications
{
	public class JobByIdSpecification : BaseSpecifications<Job>
	{
		public JobByIdSpecification(int jobId) : base(j => j.Id == jobId)
		{
			AddIncludes(j => j.Employer);
		}
	}
}
