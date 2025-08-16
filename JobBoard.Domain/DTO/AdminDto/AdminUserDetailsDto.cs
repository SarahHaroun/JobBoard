using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AdminDto
{
	public class AdminUserDetailsDto
	{
		public string Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? UserName { get; set; }
		public string? PhoneNumber { get; set; }
		public UserType UserType { get; set; }
		public bool IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public AdminSeekerProfileDto? SeekerProfile { get; set; }
		public AdminEmpProfileDto? EmployerProfile { get; set; }
		public string FullName => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
			? $"{FirstName} {LastName}" : "N/A";
	}
}
