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
	public class SavedJobWithFilterSpecification : BaseSpecifications<SavedJob>
	{
		public SavedJobWithFilterSpecification(int seekerId, SavedJobFilterParams filterParams)
		: base(s =>
			s.SeekerId == seekerId && 
			(string.IsNullOrEmpty(filterParams.SearchValue) ||
			(
				s.Job.Title.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
				s.Job.Employer.CompanyName.ToLower().Contains(filterParams.SearchValue.ToLower()) ||
				s.Job.Employer.CompanyLocation.ToLower().Contains(filterParams.SearchValue.ToLower())
			))
		)
		{
			AddIncludes(s => s.Job);
			AddIncludes(s => s.Job.Employer);

			switch (filterParams.SortingOption)
			{
				case SortingDateOptions.DateAsc:
					AddOrderBy(s => s.SavedAt);
					break;
				default:
					AddOrderByDesc(s => s.SavedAt);
					break;
			}
		}

		public SavedJobWithFilterSpecification(int seekerId, int jobId)
			: base(s => s.SeekerId == seekerId && s.JobId == jobId)
		{
			AddIncludes(s => s.Job);
			AddIncludes(s => s.Job.Employer);
		}

		public SavedJobWithFilterSpecification(int savedJobId)
			: base(s => s.Id == savedJobId)
		{
			AddIncludes(s => s.Job);
			AddIncludes(s => s.Job.Employer);
		}
	}
}
