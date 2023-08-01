using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.Infra.Data.EF.Repositories;
using CodeFlix.Catalog.Infra.Data.EF;
using MediatR;

namespace CodeFlix.Catalog.Api.Configuration
{
    public static class UseCasesConfiguration
    {
        public static IServiceCollection AddUseCases(
       this IServiceCollection services
   )
        {
            services.AddMediatR(typeof(CreateCategory));
            services.AddRepositories();
            return services;
        }

        private static IServiceCollection AddRepositories(
                this IServiceCollection services
            )
        {
            services.AddTransient<
                ICategoryRepository, CategoryRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
