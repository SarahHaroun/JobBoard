using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.AdminSpecifications
{
	// Admin Specifications
	public class AllSeekersSpecification : BaseSpecifications<SeekerProfile>
	{
		public AllSeekersSpecification()
		{
			AddIncludes(s => s.User);
		}
	}

	public class AllEmployersSpecification : BaseSpecifications<EmployerProfile>
	{
		public AllEmployersSpecification()
		{
			AddIncludes(e => e.User);
		}
	}

	public class AllJobsSpecification : BaseSpecifications<Job>
	{
		public AllJobsSpecification()
		{
			AddIncludes(j => j.Employer);
		}
	}

	public class PendingJobsSpecification : BaseSpecifications<Job>
	{
		public PendingJobsSpecification() : base(j => !j.IsApproved)
		{
			AddIncludes(j => j.Employer);
		}
	}

	public class JobByIdSpecification : BaseSpecifications<Job>
	{
		public JobByIdSpecification(int jobId) : base(j => j.Id == jobId)
		{
			AddIncludes(j => j.Employer);
		}
	}
}
