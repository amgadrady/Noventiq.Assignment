using NoventiqAssignment.Services;
using NoventiqAssignment.UnitOfWork;

namespace NoventiqAssignment.API
{
    public static class InstallServices
    {
        public static void AddMedsultoServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Scan(selector =>
                 selector.FromAssemblies((typeof(ITokenService).Assembly))
                 .AddClasses(s => s.InNamespaceOf<ITokenService>())
                 .AsImplementedInterfaces()
                 .WithScopedLifetime()
             );
            services.Scan(selector =>
             selector.FromAssemblies((typeof(IUnitOfWork).Assembly))
             .AddClasses(s => s.InNamespaceOf<IUnitOfWork>())
             .AsImplementedInterfaces()
             .WithScopedLifetime()
         );
            services.AddHttpContextAccessor();
        }

    }
}
