using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class EmployerProfile
    {
		public int Id { get; set; }

		public string CompanyName { get; set; }

		public string? CompanyLocation { get; set; }
		public string? CompanyImage  { get; set; }
		public string? Companylogo  { get; set; }
		public string? WebsiteLink { get; set; }
		public string? Industry { get; set; }
		public string? CompanyDescription  { get; set; }
		public string? Companymission  { get; set; }
		public int? EmployeesNumber { get; set; }
		public int FoundedAt { get; set; }


		/*------------------------user--------------------------*/
		[ForeignKey("User")]
		public string? UserId { get; set; }
		public ApplicationUser User { get; set; }

		/*------------------------job--------------------------*/
		public List<Job>? PostedJobs { get; set; }
	}
}
