using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications.JobSpecifications
{
	public class SkillsByIdsSpecifications : BaseSpecifications<Skill>
	{
		public SkillsByIdsSpecifications(IEnumerable<int> ids)
			: base(s => ids.Contains(s.Id))
		{
		}
	}
	
}
