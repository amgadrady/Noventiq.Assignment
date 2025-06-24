using NoventiqAssignment.Repository;

namespace NoventiqAssignment.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity>? Repository<TEntity>() where TEntity : class;
        Task SaveChangesAsync();

    }
}
