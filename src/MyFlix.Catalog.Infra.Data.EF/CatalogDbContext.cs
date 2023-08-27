using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Infra.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MyFlix.Catalog.Infra.Data.EF.Models;

namespace MyFlix.Catalog.Infra.Data.EF
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Genre> Genres => Set<Genre>();

        public DbSet<GenresCategories> GenresCategories => Set<GenresCategories>();

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // here appies the configuration for all
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            // here applies the configuration only for what you want
            // more control
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new GenreConfiguration());

            builder.ApplyConfiguration(new GenresCategoriesConfiguration());
        }
    }
}
