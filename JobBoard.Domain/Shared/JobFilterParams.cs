using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Shared.SortingOptions;

namespace JobBoard.Domain.Shared
{
    public class JobFilterParams
    {
		public int? CategoryId { get; set; } 
		public int? SkillId { get; set; }
		public int? EmployerId { get; set; }
		public WorkplaceType? WorkplaceType { get; set; }
		public JobType? JobType { get; set; }
		public ExperienceLevel? ExperienceLevel { get; set; }
		public EducationLevel? EducationLevel { get; set; }
		public bool? IsActive { get; set; }
		public JobSortingOptions SortingOption { get; set; }
		public string? SearchValue { get; set; }
		public string? SearchByLocationValue { get; set; }
		private int _pageIndex = 1;
		public int PageIndex
		{
			get => _pageIndex;
			set => _pageIndex = (value < 1) ? 1 : value;
		}

		private int _pageSize = 10;
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > 100) ? 100 : (value < 1 ? 1 : value); 
		}
	}
}
