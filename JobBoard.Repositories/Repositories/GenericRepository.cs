﻿using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Repositories.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories.Repositories
{
	class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		private readonly ApplicationDbContext _context;

		public GenericRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			if(typeof(TEntity) == typeof(Job))
			{
				var jobs = await _context.Jobs.Include(j => j.Employer)
					.Include(j => j.Categories)
					.Include(j => j.Skills).ToListAsync();
				return (IEnumerable<TEntity>) jobs;
			}
			return await _context.Set<TEntity>().ToListAsync();
		}

		public async Task<TEntity> GetByIdAsync(int id)
		{
			if (typeof(TEntity) == typeof(Job) && id is int jobId)
			{
				var job = await _context.Jobs.Include(j => j.Employer)
					.Include(j => j.Categories)
					.Include(j => j.Skills)
					.FirstOrDefaultAsync(j => j.Id == jobId);
				return job as TEntity;
			}
			return await _context.Set<TEntity>().FindAsync(id);
		}

        public async Task<TEntity> GetByIdWithIncludeAsync(int id, params string[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params string[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(TEntity entity)
		{
			await _context.AddAsync(entity);
		}

		public void Update(TEntity entity)
		{
			_context.Update(entity);
		}

		public void Delete(TEntity entity)
		{
			_context.Remove(entity);
		}
	}
}
