using JobBoard.Domain.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories
{
    class SpecificationEvaluator
    {

		public static IQueryable<TEntity> CreateQuery<TEntity>(
		IQueryable<TEntity> inputQuery,
		ISpecifications<TEntity> specifications,
		bool isForCount = false) where TEntity : class
		{
			var query = inputQuery;

			// If specifications is null, return the original query
			if (specifications == null)
				return query;

			// Apply where criteria
			if (specifications.Criteria is not null)
				query = query.Where(specifications.Criteria);

			// Apply includes 
			if (!isForCount && specifications.Includes is not null && specifications.Includes.Count > 0)
				query = specifications.Includes.Aggregate(query, (curr, include) => curr.Include(include));

			if (!isForCount)
			{
				if (specifications.Order is not null)
					query = query.OrderBy(specifications.Order);

				if (specifications.OrderDesc is not null)
					query = query.OrderByDescending(specifications.OrderDesc);

				if (specifications.IsPaginationEnabled)
					query = query.Skip(specifications.Skip).Take(specifications.Take);
			}

			return query;
		}
	}
}
