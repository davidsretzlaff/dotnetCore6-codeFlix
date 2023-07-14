using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CodeFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CategoryRepository
{
    [CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture> { }
    public class CategoryRepositoryTestFixture : BaseFixture
    {
        public Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRamdomBoolean()
            );

        public string GetValidCategoryName()
        {
            var categoryName = "";
            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];
            if (categoryName.Length > 255)
                categoryName = categoryName[..255];
            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription =
                Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 10_000)
                categoryDescription =
                    categoryDescription[..10_000];
            return categoryDescription;
        }

        public bool getRamdomBoolean()
         => new Random().NextDouble() < 0.5;

        public CatalogDbContext CreateDbContext()
        {
            var dbContext = new CatalogDbContext(
                new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
            );
        }
    }
}
