using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.JobsDto
{
	public class RecentJobDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime PostedDate { get; set; }
		public int ApplicationsCount { get; set; }
		public bool IsActive { get; set; }
		public DateTime ExpireDate { get; set; }
		public string PostedAgo { get; set; }
		public string Status
		{
			get
			{
				if (ExpireDate < DateTime.Now)
					return "Expired";

				return IsActive ? "Active" : "Filled";
			}
		}
	}

}
