using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.IntegrationTest.Base;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.UnitOfWork
{
    [CollectionDefinition(nameof(UnitOfWorkTestFixture))]
    public class UnitOfWorkTestFixtureCollection
        : ICollectionFixture<UnitOfWorkTestFixture>
    { }

    public class UnitOfWorkTestFixture
        : BaseFixture
    {
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

        public bool getRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRandomBoolean()
            );

        public List<Category> GetExampleCategoriesList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetExampleCategory()).ToList();

    }
}
