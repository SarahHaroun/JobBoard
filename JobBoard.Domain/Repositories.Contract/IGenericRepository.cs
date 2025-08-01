﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Repositories.Contract
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<TEntity> GetByIdAsync(int id);
		public  Task<TEntity> GetByIdWithIncludeAsync(int id, params string[] includeProperties);
		public Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params string[] includeProperties);
        public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);


        Task AddAsync(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntity entity);
	}
}
