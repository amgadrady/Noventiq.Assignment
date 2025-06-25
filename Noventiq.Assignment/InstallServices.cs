using NoventiqAssignment.Services;

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

        }

    }
}
