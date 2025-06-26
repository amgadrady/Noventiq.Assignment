using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoventiqAssignment.DB.Models;
using static System.Net.WebRequestMethods;

namespace NoventiqAssignment.DB.Context
{
    public class NoventiqContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public NoventiqContext(DbContextOptions<NoventiqContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductTranslation> ProductTranslations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>(entity =>
            {


                entity.Property(e => e.FirstName).HasMaxLength(150);
                entity.Property(e => e.LastName).HasMaxLength(150);
            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(250);
            });
        }


    }
}
