using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ApplicationDto
{
    public class JobSummaryDto
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public string CompanyName { get; set; }
		public string CompanyLocation { get; set; }
		public decimal? Salary { get; set; }
		public  int EmployerId { get; set; }
	}
}
