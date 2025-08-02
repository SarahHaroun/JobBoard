using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
	public class JobsWithFilterSpecifications : BaseSpecifications<Job>
	{
		public JobsWithFilterSpecifications()
		{
			AddIncludes(J => J.Skills);
			AddIncludes(J => J.Employer);
		}
		public JobsWithFilterSpecifications(int id) : base(J => J.Id == id)
		{
			{
				AddIncludes(J => J.Skills);
				AddIncludes(J => J.Categories);
				AddIncludes(J => J.Employer);
			}
		}
	}
}
