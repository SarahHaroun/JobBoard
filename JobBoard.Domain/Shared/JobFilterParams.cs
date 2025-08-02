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
		public string? Title { get; set; }
		public int? categoryId { get; set; } 
		public int? skillId { get; set; }
		public int? employerId { get; set; }
		public WorkplaceType? WorkplaceType { get; set; }
		public JobType? JobType { get; set; }
		public ExperienceLevel? ExperienceLevel { get; set; }
		public EducationLevel? EducationLevel { get; set; }
		public bool? IsActive { get; set; }
		public SortingOptions sortingOption { get; set; }
	}
}
