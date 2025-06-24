using NoventiqAssignment.DB.Context;
using NoventiqAssignment.Repository;
using System.Collections;

namespace NoventiqAssignment.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private NoventiqContext noventiqContext;
        private Hashtable? repositories;
        public UnitOfWork(NoventiqContext _noventiqContext)
        {
            noventiqContext = _noventiqContext;
        }
        public IGenericRepository<TEntity>? Repository<TEntity>() where TEntity : class
        {
            repositories ??= new Hashtable();

            var type = typeof(TEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance =
                    Activator.CreateInstance(
                        repositoryType
                        .MakeGenericType(typeof(TEntity)), noventiqContext);

                repositories.Add(type, repositoryInstance);
            }

            return repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task SaveChangesAsync()
        {
            await noventiqContext.SaveChangesAsync();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    noventiqContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
