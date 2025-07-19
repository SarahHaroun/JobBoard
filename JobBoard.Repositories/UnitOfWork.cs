using JobBoard.Domain.Repositories.Contract;
using JobBoard.Repositories.Persistence;
using JobBoard.Repositories.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Repositories
{
	class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private readonly Hashtable _repositories;

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			_repositories = new Hashtable();
		}
		public async Task<int> CompleteAsync()
			=> await _context.SaveChangesAsync();
		

		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
		{
			var repoType = typeof(TEntity).Name;

			//Check if the repository already exists in the hashtable
			if (!_repositories.ContainsKey(repoType)) { 
				var repo = new GenericRepository<TEntity>(_context);
				_repositories.Add(repo, repoType);
			}
			return _repositories[repoType] as IGenericRepository<TEntity>;
		}
	}
}
