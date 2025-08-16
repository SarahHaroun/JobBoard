using JobBoard.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AdminDto
{
	public class AdminSeekerProfileDto : SeekerProfile
	{
		public bool IsActive { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string? UserName { get; set; }
		public string FullName => !string.IsNullOrEmpty(Name) ? Name : "N/A";
		public int ApplicationsCount { get; set; }
		public DateTime? LastLoginDate { get; set; }
	}


}
