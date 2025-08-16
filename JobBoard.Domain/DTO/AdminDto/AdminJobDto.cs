using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AdminDto
{
	public class AdminJobDto : JobListDto
	{
		// Additional admin-specific properties
		public decimal? Salary { get; set; }               
		public DateTime? ExpireDate { get; set; }
		public string? EducationLevel { get; set; }
		public string? ExperienceLevel { get; set; }
		public int? MinTeamSize { get; set; }
		public int? MaxTeamSize { get; set; }
		public bool IsActive { get; set; }
		public string? Requirements { get; set; }
		public string? Responsabilities { get; set; }
		public string? Benefits { get; set; }
		public bool IsApproved { get; set; } = false;
		public string? CompanyImage { get; set; }
		public string? Website { get; set; }
		public string? Industry { get; set; }
		public string? CompanyDescription { get; set; }
		public string? CompanyMission { get; set; }
		public string? EmployeeRange { get; set; }
		public int? EstablishedYear { get; set; }
		public List<string> Categories { get; set; } = new();

		// Computed properties
		public bool IsExpired => ExpireDate.HasValue && ExpireDate <= DateTime.Now;
		public string SalaryDisplay => base.Salary.HasValue ? $"${base.Salary:N0}" : "Not specified";
		public string StatusDisplay => IsApproved ? (IsActive ? "Active" : "Inactive") : "Pending";
	}
}
