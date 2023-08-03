using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.Category.CreateCategory;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.UnitTests.Common;
using Moq;

namespace MyFlix.Catalog.UnitTests.Application.Category.Common
{
    public abstract class CategoryUseCasesBaseFixture
        : BaseFixture
    {

        public Mock<ICategoryRepository> GetRepositoryMock() => new();
        public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

        public CreateCategoryInput GetInput()
            => new(GetValidCategoryName(),
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
        public DomainEntity.Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRamdomBoolean()
        );
        public bool getRamdomBoolean()
           => new Random().NextDouble() < 0.5;

    }
}
