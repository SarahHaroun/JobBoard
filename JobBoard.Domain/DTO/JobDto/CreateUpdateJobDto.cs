using JobBoard.Domain.Entities.Enums;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using JobBoard.Domain.Attributes;

namespace JobBoard.Domain.DTO.JobDto
{
	[TeamSizeValidationAttribute]
	public class CreateUpdateJobDto
	{
		[Required]
		[StringLength(200)]
		public string Title { get; set; }
		public string Description { get; set; }


		[Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
		public decimal? Salary { get; set; }

		[Required]
		public WorkplaceType WorkplaceType { get; set; }

		[Required]
		public JobType JobType { get; set; }
		public DateTime? ExpireDate { get; set; }
		public EducationLevel? EducationLevel { get; set; }

		[Range(1, 100)]
		public int? MinTeamSize { get; set; }

		[Range(1, 100)]
		public int? MaxTeamSize { get; set; }
		public ExperienceLevel? ExperienceLevel { get; set; }
		public string? Requirements { get; set; }
		public bool IsActive { get; set; } = true;

		public List<int> CategoryIds { get; set; } = [];
		public List<int> SkillIds { get; set; } = [];
	}
}
