using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
	public class JobsByDateRangeSpecification : BaseSpecifications<Job>
	{
		public JobsByDateRangeSpecification(DateTime start, DateTime end)
			: base(j => j.PostedDate >= start && j.PostedDate < end)
		{
		}

		public JobsByDateRangeSpecification(DateTime start, DateTime end, bool onlyApproved, bool onlyActive)
			: base(j =>
				j.PostedDate >= start &&
				j.PostedDate < end &&
				(!onlyApproved || j.IsApproved) &&
				(!onlyActive || j.IsActive))
		{
		}
	}

}
