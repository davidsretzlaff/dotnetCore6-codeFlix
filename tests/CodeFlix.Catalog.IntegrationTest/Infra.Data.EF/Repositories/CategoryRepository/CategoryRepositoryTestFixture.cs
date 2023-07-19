using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Infra.Data.EF;
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
                GetRamdomBoolean()
            );

        public List<Category> GetExampleCategoriesList(int length = 10)
            => Enumerable.Range(1, length)
            .Select(_ => GetExampleCategory()).ToList();

        public List<Category> GetExampleCategoriesListWithNames(List<string> names)
            => names.Select(name =>
            {
                var category = GetExampleCategory();
                category.Update(name);
                return category;
            }).ToList();
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

        public bool GetRamdomBoolean()
         => new Random().NextDouble() < 0.5;

        public CatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new CatalogDbContext(
                new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
            );
            
            if(preserveData == false)
                context.Database.EnsureDeleted();

            return context;
        }
    }
}
