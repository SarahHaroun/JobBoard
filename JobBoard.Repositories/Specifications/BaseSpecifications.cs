using JobBoard.Domain.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Specifications
{
	public abstract class BaseSpecifications<TEntity> : ISpecifications<TEntity> where TEntity : class
	{
		public BaseSpecifications() { }
		public BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExp) {
			Criteria = criteriaExp;
		}

		public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

		public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

		protected void AddIncludes(Expression<Func<TEntity, object>> include)
		{
			if(include is not null) 
				Includes.Add(include);
		}
	}
}
