using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.DashboardSpecifications
{
	public class JobsExpiringSoonSpecification : BaseSpecifications<Job>
	{
		public JobsExpiringSoonSpecification(int employerId, DateTime expiringDate)
			: base(j => j.EmployerId == employerId &&
						j.IsApproved && j.IsActive &&
						j.ExpireDate <= expiringDate &&
						j.ExpireDate.Value >= DateTime.Now)
		{


		}
	} 
}
