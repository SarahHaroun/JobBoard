using JobBoard.Domain.DTO.EmployerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AdminDto
{
	public class AdminEmpProfileDto : EmpProfileDto
	{
		public bool IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? UserName { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string FullName => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
			? $"{FirstName} {LastName}" : "N/A";
		public int JobsPostedCount { get; set; }
		public int ActiveJobsCount { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public string StatusDisplay => IsActive ? "Active" : "Inactive";
	}
}
