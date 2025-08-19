using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
	// Admin Specifications
	public class AllSeekersSpecification : BaseSpecifications<SeekerProfile>
	{
		public AllSeekersSpecification()
		{
			// Include User data
			AddIncludes(s => s.User);

			// Include all related collections
			AddIncludes(s => s.Skills);
			AddIncludes(s => s.SeekerEducations);
			AddIncludes(s => s.SeekerExperiences);
			AddIncludes(s => s.SeekerTraining);
			AddIncludes(s => s.seekerCertificates);
			AddIncludes(s => s.seekerInterests);
			AddIncludes(s => s.UserApplications);
			
			
			AddOrderByDesc(e => e.Id);

		}
	}

	public class AllEmployersSpecification : BaseSpecifications<EmployerProfile>
	{
		public AllEmployersSpecification()
		{
			AddIncludes(e => e.User);
			
			
			AddOrderByDesc(e => e.Id);
		}
	}

	public class AllJobsSpecification : BaseSpecifications<Job>
	{
		public AllJobsSpecification()
		{
			AddIncludes(j => j.Employer);
			AddIncludes(j => j.Categories);
			AddIncludes(j => j.Skills);
			AddOrderByDesc(j => !j.IsApproved);
		}
	}

	public class PendingJobsSpecification : BaseSpecifications<Job>
	{
		public PendingJobsSpecification() : base(j => !j.IsApproved)
		{
			AddIncludes(j => j.Employer);
			AddIncludes(j => j.Categories);
			AddIncludes(j => j.Skills);
			AddOrderByDesc(j => j.PostedDate);
		}
	}

	public class JobByIdSpecification : BaseSpecifications<Job>
	{
		public JobByIdSpecification(int jobId) : base(j => j.Id == jobId)
		{
			AddIncludes(j => j.Employer);
			AddIncludes(j => j.Categories);
			AddIncludes(j => j.Skills);
		}
	}
}
