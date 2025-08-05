using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.EmployerDto
{
	public class EmployerSeedDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string CompanyName { get; set; }
		public string Website { get; set; }
		public string CompanyLocation { get; set; }
		public int EstablishedYear { get; set; }
		public string CompanyDescription { get; set; }
		public string? CompanyImage { get; set; }
		public string Companylogo { get; set; }
		public string Companymission { get; set; }
		public int EmployeesNumber { get; set; }
		public string Industry { get; set; }

	}
}

