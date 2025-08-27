using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AdminDto
{
    public class EmployerListDto
    {
		public int Id { get; set; }
		public string? Email { get; set; }
		public string CompanyName { get; set; }
	}
}
