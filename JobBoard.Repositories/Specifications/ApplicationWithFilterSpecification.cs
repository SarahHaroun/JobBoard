using JobBoard.Domain.Entities;
using JobBoard.Domain.Shared;
using JobBoard.Domain.Shared.SortingOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
	public class ApplicationWithFilterSpecification : BaseSpecifications<Application>
	{
		public ApplicationWithFilterSpecification(ApplicationFilterParams filterParams)
		   : base(a =>
		   (!filterParams.JobId.HasValue || a.JobId == filterParams.JobId) &&
		   (!filterParams.ApplicantId.HasValue || a.ApplicantId == filterParams.ApplicantId) &&
		   (!filterParams.Status.HasValue || a.Status == filterParams.Status) &&
		   (!filterParams.AppliedDateFrom.HasValue || a.AppliedDate >= filterParams.AppliedDateFrom) &&
		   (!filterParams.AppliedDateTo.HasValue || a.AppliedDate <= filterParams.AppliedDateTo) &&
		   (string.IsNullOrWhiteSpace(filterParams.SearchValue) ||
			a.FullName.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
			a.CurrentJobTitle.ToLower().Contains(filterParams.SearchValue.ToLower())))
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Applicant);
			switch (filterParams.SortingOption)
			{
				case SortingDateOptions.DateAsc:
					AddOrderBy(a => a.AppliedDate);
					break;
				default:
					AddOrderByDesc(a => a.AppliedDate);
					break;
			}
		}

		public ApplicationWithFilterSpecification(int id) : base(a => a.Id == id)
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Applicant);
		}

		public ApplicationWithFilterSpecification(int applicantId, int jobId)
		: base(a => a.ApplicantId == applicantId && a.JobId == jobId)
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Applicant);
		}

		public ApplicationWithFilterSpecification(int applicantId, bool isApplicantId = true)
		: base(a => a.ApplicantId == applicantId)
		{
			AddIncludes(a => a.Job);
			AddIncludes(a => a.Applicant);
			AddOrderByDesc(a => a.AppliedDate);
		}
	}
}

