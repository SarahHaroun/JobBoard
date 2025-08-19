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
	   (!filterParams.Status.HasValue || a.Status == filterParams.Status) &&
	   (!filterParams.AppliedDateFrom.HasValue || a.AppliedDate >= filterParams.AppliedDateFrom) &&
	   (!filterParams.AppliedDateTo.HasValue || a.AppliedDate <= filterParams.AppliedDateTo) &&
	   (string.IsNullOrWhiteSpace(filterParams.SearchValue) ||
		a.FullName.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
		a.CurrentJobTitle.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
		a.Job.Title.ToLower().Contains(filterParams.SearchValue.ToLower())))
	{
		AddIncludes(a => a.Job);
		AddIncludes(a => a.Job.Employer);
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
}
