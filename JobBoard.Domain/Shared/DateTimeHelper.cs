using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Shared
{
	public static class DateTimeHelper
	{
		public static string CalculateTimeAgo(DateTime postedDate)
		{
			var timeSpan = DateTime.Now - postedDate;

			if (timeSpan.Days == 0)
				return "Today";
			else if (timeSpan.Days == 1)
				return "1 day ago";
			else if (timeSpan.Days < 7)
				return $"{timeSpan.Days} days ago";
			else if (timeSpan.Days < 14)
				return "1 week ago";
			else if (timeSpan.Days < 30)
				return $"{timeSpan.Days / 7} weeks ago";
			else if (timeSpan.Days < 60)
				return "1 month ago";
			else
				return $"{timeSpan.Days / 30} months ago";
		}
	}

}
