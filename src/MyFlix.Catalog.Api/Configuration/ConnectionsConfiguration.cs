using MyFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace MyFlix.Catalog.Api.Configuration
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAppConections(
            this IServiceCollection services)
        {
            services.AddDbConnection();
            return services;
        }

        private static IServiceCollection AddDbConnection(
            this IServiceCollection services)
        {
            services.AddDbContext<CatalogDbContext>(
                options => options.UseInMemoryDatabase(
                    "InMemory-DSV-Database"
                )
            );
            return services;
        }
    }
}
