using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.UnitTests.Common;
using Moq;
using System;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetcagegoryTextFixtureCollection : ICollectionFixture<GetCategoryTestFixture> {}
    public class GetCategoryTestFixture : BaseFixture
    {
        public Mock<ICategoryRepository> GetRepositoryMock() => new();

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
            var categoryDescription = Faker.Commerce.ProductDescription();
            if(categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];
            return categoryDescription; ;
        }

        public Category GetValidCategory()
        {
            return new
            (
                GetValidCategoryName(),
                GetValidCategoryDescription()
            );
        }
    }
}
