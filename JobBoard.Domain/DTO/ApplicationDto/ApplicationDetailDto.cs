using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ApplicationDto
{
	public class ApplicationDetailDto
	{
		public string? CoverLetter { get; set; }
		public DateTime AppliedDate { get; set; }
		public ApplicationStatus Status { get; set; }
		public int JobId { get; set; }
	}
}
