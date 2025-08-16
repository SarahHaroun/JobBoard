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
			: base(j => j.IsApproved &&
			(string.IsNullOrWhiteSpace(filterParams.SearchValue) || j.Title.ToLower().Contains(filterParams.SearchValue.ToLower())) &&
			(string.IsNullOrWhiteSpace(filterParams.SearchByLocationValue) || j.Employer.CompanyLocation.ToLower().Contains(filterParams.SearchByLocationValue.ToLower())) &&
			(!filterParams.CategoryId.HasValue || j.Categories.Any(jc => jc.Id == filterParams.CategoryId)) &&
			(!filterParams.SkillId.HasValue || j.Skills.Any(js => js.Id == filterParams.SkillId)) &&
			(!filterParams.EmployerId.HasValue || j.EmployerId == filterParams.EmployerId) &&
			(!filterParams.WorkplaceType.HasValue || j.WorkplaceType == filterParams.WorkplaceType) &&
			(!filterParams.JobType.HasValue || j.JobType == filterParams.JobType) &&
			(!filterParams.ExperienceLevel.HasValue || j.ExperienceLevel == filterParams.ExperienceLevel) &&
			(!filterParams.EducationLevel.HasValue || j.EducationLevel == filterParams.EducationLevel) &&
			(!filterParams.IsActive.HasValue || j.IsActive == filterParams.IsActive))
		{
			AddIncludes(j => j.Skills);
			AddIncludes(j => j.Employer);

			switch (filterParams.SortingOption)
			{
				case JobSortingOptions.SalaryAsc:
					AddOrderBy(j => j.Salary);
					break;
				case JobSortingOptions.SalaryDesc:
					AddOrderByDesc(j => j.Salary);
					break;
				case JobSortingOptions.DateAsc:
					AddOrderBy(j => j.PostedDate);
					break;
				default:
					AddOrderByDesc(j => j.PostedDate);
					break;
			}

			var skip = filterParams.PageSize * (filterParams.PageIndex - 1);
			AddPagination(skip, filterParams.PageSize);
		}
		public JobsWithFilterSpecifications(int id) : base(j => j.Id == id && j.IsApproved)
		{
			{
				AddIncludes(j => j.Skills);
				AddIncludes(j => j.Categories);
				AddIncludes(j => j.Employer);
			}
		}
	}
}
