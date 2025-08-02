using JobBoard.Domain.Entities;
using JobBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
	public class JobsWithFilterSpecifications : BaseSpecifications<Job>
	{
		public JobsWithFilterSpecifications(JobFilterParams filterParams)
			: base(j =>
			(!filterParams.categoryId.HasValue || j.Categories.Any(jc => jc.Id == filterParams.categoryId)) &&
			(!filterParams.skillId.HasValue || j.Skills.Any(js => js.Id == filterParams.skillId)) &&
			(!filterParams.employerId.HasValue || j.EmployerId == filterParams.employerId) &&
			(!filterParams.WorkplaceType.HasValue || j.WorkplaceType == filterParams.WorkplaceType) &&
			(!filterParams.JobType.HasValue || j.JobType == filterParams.JobType) &&
			(!filterParams.ExperienceLevel.HasValue || j.ExperienceLevel == filterParams.ExperienceLevel) &&
			(!filterParams.EducationLevel.HasValue || j.EducationLevel == filterParams.EducationLevel) &&
			(!filterParams.IsActive.HasValue || j.IsActive == filterParams.IsActive))
		{
			AddIncludes(j => j.Skills);
			AddIncludes(j => j.Employer);

			switch (filterParams.sortingOption)
			{
				case SortingOptions.SalaryAsc:
					AddOrderBy(j => j.Salary);
					break;
				case SortingOptions.SalaryDesc:
					AddOrderByDesc(j => j.Salary);
					break;
				case SortingOptions.DateAsc:
					AddOrderBy(j => j.PostedDate);
					break;
				default:
					AddOrderByDesc(j => j.PostedDate);
					break;
			}
		}
		public JobsWithFilterSpecifications(int id) : base(j => j.Id == id)
		{
			{
				AddIncludes(j => j.Skills);
				AddIncludes(j => j.Categories);
				AddIncludes(j => j.Employer);
			}
		}
	}
}
