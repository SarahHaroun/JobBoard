using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Shared
{
    public class JobFilterParams
    {
		//public string? Title { get; set; }
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
	}
}
