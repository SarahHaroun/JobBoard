using JobBoard.Domain.Entities;
using JobBoard.Domain.Shared;
using JobBoard.Domain.Shared.SortingOptions;

namespace JobBoard.Repositories.Specifications
{
	public class EmployerJobsWithFilterSpecification : BaseSpecifications<Job>
	{
		public EmployerJobsWithFilterSpecification(int employerId, EmployerJobFilterParams filterParams)
			: base(j => j.EmployerId == employerId &&
					   j.IsApproved &&
					   // Search by title
					   (string.IsNullOrWhiteSpace(filterParams.SearchValue) ||
						j.Title.ToLower().Contains(filterParams.SearchValue.ToLower())) &&
					   // Status filter
					   (!filterParams.Status.HasValue ||
						(filterParams.Status == EmployerJobStatus.Active &&
						 j.IsActive &&
						 (!j.ExpireDate.HasValue || j.ExpireDate > DateTime.Now)) ||
						(filterParams.Status == EmployerJobStatus.Filled && !j.IsActive) ||
						(filterParams.Status == EmployerJobStatus.Expired &&
						 j.ExpireDate.HasValue &&
						 j.ExpireDate < DateTime.Now)))
		{
			AddIncludes(j => j.JobApplications);
			AddIncludes(j => j.Employer);

			// Sorting
			switch (filterParams.SortingOption)
			{
				case EmployerJobSortingOptions.ApplicationsCountDesc:
					AddOrderByDesc(j => j.JobApplications.Count);
					break;
				case EmployerJobSortingOptions.PostedDateAsc:
					AddOrderBy(j => j.PostedDate);
					break;
				default: 
					AddOrderByDesc(j => j.PostedDate);
					break;
			}
		}
	}
}