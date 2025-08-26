using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
    // Admin Specifications

    /////////////////////////get all seekers///////////////////////
    public class AllSeekersSpecification : BaseSpecifications<SeekerProfile>
	{
		public AllSeekersSpecification()
		{
			// Include User data
			AddIncludes(s => s.User);
			AddIncludes(s => s.Skills);
			AddOrderByDesc(e => e.Id);

		}

	}

	/////////////////////////get seeker by id///////////////////////
	public class SeekerByUserIdSpecification : BaseSpecifications<SeekerProfile>
	{
		public SeekerByUserIdSpecification(string seekerId) : base(s => s.UserId == seekerId)
		{
			AddIncludes(s => s.User);
			AddIncludes(s => s.seekerCertificates);
			AddIncludes(s => s.seekerInterests);
			AddIncludes(s => s.Skills);
			AddIncludes(s => s.SeekerTraining);

        }
	
	}

    /////////////////////////get all employers///////////////////////
    public class AllEmployersSpecification : BaseSpecifications<EmployerProfile>
	{
		public AllEmployersSpecification()
		{
			AddIncludes(e => e.User);	
			AddOrderByDesc(e => e.Id);
		}
	}

	
	/////////////////////////get employer by id///////////////////////
	public class EmployerByUserIdSpecification : BaseSpecifications<EmployerProfile>
	{
		public EmployerByUserIdSpecification(string employerId) : base(e => e.UserId == employerId)
		{
			AddIncludes(e => e.User);
			
        }
	}

	////////////////////////get jobs related employer//////////////////////////
    public class JobsByEmployerIdSpecification : BaseSpecifications<Job>
	{
		public JobsByEmployerIdSpecification(int employerId) : base(j => j.EmployerId == employerId)
		{
			AddIncludes(j => j.JobApplications);
		}
    }
    /////////////////////////get all jobs///////////////////////

    public class AllJobsSpecification : BaseSpecifications<Job>
	{
		public AllJobsSpecification()
		{
			AddIncludes(j => j.Employer);
			AddIncludes(j => j.Categories);
			AddIncludes(j => j.Skills);
			AddOrderByDesc(j => !j.IsApproved);
			AddThenByDesc(j => j.PostedDate);
		}
	}

    /////////////////////////get pending jobs///////////////////////
    public class PendingJobsSpecification : BaseSpecifications<Job>
	{
		public PendingJobsSpecification() : base(j => !j.IsApproved)
		{
			AddIncludes(j => j.Employer.User);
			AddIncludes(j => j.Categories);
			AddIncludes(j => j.Skills);
			AddOrderByDesc(j => j.PostedDate);
		}
	}

    /////////////////////////get job by id///////////////////////
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
