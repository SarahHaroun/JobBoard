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
		public string? Website { get; set; }
		public int EstablishedYear { get; set; }


		/*------------------------user--------------------------*/
		[ForeignKey("User")]
		public string? UserId { get; set; }
		public ApplicationUser User { get; set; }

		/*------------------------job--------------------------*/
		public List<Job>? PostedJobs { get; set; }
	}
}
