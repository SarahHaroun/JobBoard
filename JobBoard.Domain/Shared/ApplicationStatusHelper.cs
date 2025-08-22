using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Shared
{
	public static class ApplicationStatusHelper
	{

		public static string GetStatusDisplay(ApplicationStatus status)
		{
			switch (status)
			{
				case ApplicationStatus.Pending:
					return "New";
				case ApplicationStatus.UnderReview:
					return "Under Review";
				case ApplicationStatus.Interviewed:
					return "Interview";
				case ApplicationStatus.Accepted:
					return "Hired";
				case ApplicationStatus.Rejected:
					return "Rejected";
				default:
					return "Unknown";
			}
		}

	};


}