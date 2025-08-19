using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
    public class CategoriesByIdsSpecifications : BaseSpecifications<Category>
    {
		public CategoriesByIdsSpecifications(IEnumerable<int> ids)
		: base(c => ids.Contains(c.Id))
		{
		}
	}
}
