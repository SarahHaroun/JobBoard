using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.EmployerDto
{
    public class EmployerSeedDto
    {
		public string CompanyName { get; set; }
		public string CompanyLocation { get; set; }
		public string? CompanyImage { get; set; }
		public string? Website { get; set; }
		public string? Industry { get; set; }
		public string? CompanyDescription { get; set; }
		public string? CompanyMission { get; set; }
		public string? EmployeeRange { get; set; }
		public int? EstablishedYear { get; set; }
		public string UserEmail { get; set; }
    }
}
