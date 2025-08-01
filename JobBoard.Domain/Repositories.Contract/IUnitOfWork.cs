﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Repositories.Contract
{
	public interface IUnitOfWork
	{
		Task<int> CompleteAsync();
		IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
	}
}
