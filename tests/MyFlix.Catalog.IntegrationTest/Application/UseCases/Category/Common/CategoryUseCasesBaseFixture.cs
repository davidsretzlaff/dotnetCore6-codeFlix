using Bogus;
using MyFlix.Catalog.IntegrationTest.Base;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Category.Common
{
    public class CategoryUseCasesBaseFixture : BaseFixture
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

        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public DomainEntity.Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );

        public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetExampleCategory()).ToList();


    }
}
