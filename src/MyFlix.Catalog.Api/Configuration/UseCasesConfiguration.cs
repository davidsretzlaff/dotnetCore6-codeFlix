using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.Category.CreateCategory;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.Infra.Data.EF;
using MediatR;

namespace MyFlix.Catalog.Api.Configuration
{
    public static class UseCasesConfiguration
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateCategory));
            services.AddRepositories();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
