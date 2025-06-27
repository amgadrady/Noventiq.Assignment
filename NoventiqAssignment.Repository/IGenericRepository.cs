using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace NoventiqAssignment.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetData(
     IList<Expression<Func<TEntity, bool>>> filters,
     List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> include,
     bool enableTracking = false);

        Task<int> ExecuteRawSqlAsync(string sql, params object[] parameters);
    }
}
