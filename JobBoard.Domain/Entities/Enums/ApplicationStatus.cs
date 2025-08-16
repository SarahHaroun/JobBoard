using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities.Enums
{

	public enum ApplicationStatus
	{
		Pending = 0,
		UnderReview = 1,
		Interviewed = 2,
		Accepted = 3,
		Rejected = 4,

	}
}
