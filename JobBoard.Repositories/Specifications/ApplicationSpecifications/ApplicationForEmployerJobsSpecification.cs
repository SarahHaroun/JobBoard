using JobBoard.Domain.Shared.SortingOptions;
using JobBoard.Domain.Shared;
using JobBoard.Repositories.Specifications;
using JobBoard.Domain.Entities;

public class ApplicationForEmployerJobsSpecification : BaseSpecifications<Application>
{
	public ApplicationForEmployerJobsSpecification(int employerId, ApplicationFilterParams filterParams)
	   : base(a =>
		   a.Job.EmployerId == employerId &&
		   (!filterParams.JobId.HasValue || a.JobId == filterParams.JobId) &&
		   (!filterParams.ApplicantId.HasValue || a.ApplicantId == filterParams.ApplicantId) &&
		   (!filterParams.Status.HasValue || a.Status == filterParams.Status))
	{
		AddIncludes(a => a.Job);
		AddIncludes(a => a.Job.Employer);
		AddIncludes(a => a.Applicant);

		// Sorting option
		switch (filterParams.SortingOption)
		{
			default:
				AddOrderByDesc(a => a.AppliedDate);
				break;
		}

		// Pagination
		var skip = filterParams.PageSize * (filterParams.PageIndex - 1);
		AddPagination(skip, filterParams.PageSize);
	}
}