using JobBoard.Domain.Entities;
using JobBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
	public class SavedJobWithFilterSpecification : BaseSpecifications<SavedJob>
	{
		public SavedJobWithFilterSpecification(SavedJobFilterParams filterParams)
		: base(s =>
		string.IsNullOrEmpty(filterParams.SearchValue) ||
		(
			s.Job.Title.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
			s.Job.Employer.CompanyName.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
			s.Job.Employer.CompanyLocation.ToLower().Contains(filterParams.SearchValue.ToLower())
		)
	)
		{
			AddIncludes(s => s.Job);
			AddIncludes(s => s.Job.Employer);

			switch (filterParams.SortingOption)
			{
				case SortingOptions.DateAsc:
					AddOrderBy(s => s.SavedAt);
					break;
				default:
					AddOrderByDesc(s => s.SavedAt);
					break;
			}
		}

		public SavedJobWithFilterSpecification(int id): base( s => s.Id ==  id)
		{
			AddIncludes(s => s.Job);
			AddIncludes(s => s.Job.Employer);
		}
	}
}
