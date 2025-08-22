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
		// Constructor for filtering with parameters
		public ApplicationWithFilterSpecification(ApplicationFilterParams filterParams)
		   : base(a =>
		   (!filterParams.JobId.HasValue || a.JobId == filterParams.JobId) &&
		   (!filterParams.ApplicantId.HasValue || a.ApplicantId == filterParams.ApplicantId) &&
		   (!filterParams.Status.HasValue || a.Status == filterParams.Status))
		   
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Applicant);

			switch (filterParams.SortingOption)
			{
				default:
					AddOrderByDesc(a => a.AppliedDate);
					break;
			}
			var skip = filterParams.PageSize * (filterParams.PageIndex - 1);
			AddPagination(skip, filterParams.PageSize);

		}
		// Constructor for getting by ID
		public ApplicationWithFilterSpecification(int id) : base(a => a.Id == id)
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Applicant);
			AddIncludes(a => a.Job.Employer);
		}

		// Constructor for checking if user applied to job
		public ApplicationWithFilterSpecification(int applicantId, int jobId)
			: base(a => a.ApplicantId == applicantId && a.JobId == jobId)
		{
		}

		// Constructor for getting applications by applicant
		public ApplicationWithFilterSpecification(int applicantId, bool isApplicantId)
			: base(a => a.ApplicantId == applicantId)
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Job.Employer);
			AddOrderByDesc(a => a.AppliedDate);
		}
	}
}

