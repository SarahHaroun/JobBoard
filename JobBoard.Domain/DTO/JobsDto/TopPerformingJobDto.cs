using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.JobsDto
{
    public class TopPerformingJobDto
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public int ApplicationsCount { get; set; }
		public DateTime PostedDate { get; set; }
	}
}
