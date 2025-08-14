using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ApplicationDto
{
    public class ApplicantSummaryDto
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Title { get; set; }
		public string? ProfileImageUrl { get; set; }
	}
}
