using JobBoard.Domain.Entities;
using JobBoard.Domain.Shared;
using JobBoard.Domain.Shared.SortingOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.ApplicationSpecifications
{
	public class ApplicationWithFilterSpecification : BaseSpecifications<Application>
	{
		// Constructor for filtering applications using filter parameters (JobId, ApplicantId, Status, Sorting, Pagination)
		public ApplicationWithFilterSpecification(ApplicationFilterParams filterParams)
		   : base(a =>
			   (!filterParams.JobId.HasValue || a.JobId == filterParams.JobId) &&
			   (!filterParams.ApplicantId.HasValue || a.ApplicantId == filterParams.ApplicantId) &&
			   (!filterParams.Status.HasValue || a.Status == filterParams.Status))
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Applicant);
			AddOrderByDesc(a => a.AppliedDate);

			// Pagination
			var skip = filterParams.PageSize * (filterParams.PageIndex - 1);
			AddPagination(skip, filterParams.PageSize);
		}

		// Constructor for getting application by Id (with Job + Applicant + Employer includes)
		public ApplicationWithFilterSpecification(int id)
			: base(a => a.Id == id)
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Applicant);
			AddIncludes(a => a.Job.Employer);
		}

		// Constructor for checking if a user has applied to a specific job
		public ApplicationWithFilterSpecification(int applicantId, int jobId)
			: base(a => a.ApplicantId == applicantId && a.JobId == jobId)
		{
		}

		// Constructor for getting applications by applicant (with Job + Employer includes)
		public ApplicationWithFilterSpecification(int applicantId, bool isApplicantId)
			: base(a => a.ApplicantId == applicantId)
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Job.Employer);
			AddOrderByDesc(a => a.AppliedDate);
		}
	}
}

