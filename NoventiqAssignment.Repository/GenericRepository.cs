using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NoventiqAssignment.DB.Context;
using System.Linq.Expressions;

namespace NoventiqAssignment.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly NoventiqContext noventiqContext;
        DbSet<TEntity> dbSet;

        public GenericRepository(NoventiqContext noventiqContext)
        {
            this.noventiqContext = noventiqContext;
            dbSet = noventiqContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetData(
      IList<Expression<Func<TEntity, bool>>> filter =null,
      List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> include=null,
   bool enableTracking = false)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null && filter.Count > 0)
                foreach (var item in filter)
                    query = query.Where(item);

            query = include.Aggregate(query, (current, include) => include(current));
            if (enableTracking)
                query = query.AsTracking();
            return await query.AsSplitQuery().ToListAsync();
        }

        public async Task<int> ExecuteRawSqlAsync(string sql, params object[] parameters)
        {
            return await noventiqContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}
