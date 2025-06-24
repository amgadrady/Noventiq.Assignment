using Microsoft.EntityFrameworkCore;

namespace NoventiqAssignment.DB.Context
{
    public class NoventiqContext : DbContext
    {
        public NoventiqContext(DbContextOptions<NoventiqContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
