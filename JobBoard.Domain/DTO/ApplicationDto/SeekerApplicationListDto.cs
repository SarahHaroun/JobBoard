using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ApplicationDto
{
    public class SeekerApplicationListDto
    {
		public int ApplicationId { get; set; }
		public int JobId { get; set; }
		public string JobTitle { get; set; }
		public string CompanyName { get; set; }
		public string CompanyLocation { get; set; }
		public string CompanyImage { get; set; }
		public string Status { get; set; }
		public string AppliedDate { get; set; }
	}
}
