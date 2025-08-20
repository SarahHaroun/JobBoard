using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.JobsDto
{
    public class PostedJobsDto
    {
		public int Id { get; set; }
		public string Title { get; set; }
		private DateTime PostedDate { get; set; }
		public DateTime? ExpireDate { get; set; }
		public int ApplicationsCount { get; set; }
		public string PostedDateFormatted => PostedDate.ToString("yyyy-MM-dd");
		public string ExpireDateFormatted => ExpireDate?.ToString("yyyy-MM-dd") ?? "No expiry";
		public bool IsExpiringSoon => ExpireDate.HasValue && ExpireDate.Value <= DateTime.Now.AddDays(7);
		public bool IsActive { get; set; }
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
