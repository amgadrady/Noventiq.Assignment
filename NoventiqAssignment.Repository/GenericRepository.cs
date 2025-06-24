using Microsoft.EntityFrameworkCore;
using NoventiqAssignment.DB.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoventiqAssignment.Repository
{
    public  class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly NoventiqContext noventiqContext ;
        DbSet<TEntity> dbSet;

        public GenericRepository(NoventiqContext noventiqContext)
        {
            this.noventiqContext = noventiqContext;
            dbSet = noventiqContext.Set<TEntity>();
        }
    }
}
