using Bogus;
using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.IntegrationTest.Base
{
    public class BaseFixture
    {
        public BaseFixture() => Faker = new Faker("pt_BR");

        protected Faker Faker { get; set; }

        public CatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new CatalogDbContext(
                new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase($"integration-tests-db")
                .Options
            );

            if (preserveData == false)
                context.Database.EnsureDeleted();

            return context;
        }

    }
}
