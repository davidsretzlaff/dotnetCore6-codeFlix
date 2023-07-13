using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.UnitTests.Common;
using Moq;

namespace CodeFlix.Catalog.UnitTests.Application.Common
{
    public abstract class CategoryUseCasesBaseFixture
        : BaseFixture
    {

        public Mock<ICategoryRepository> GetRepositoryMock() => new();
        public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

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
        public CreateCategoryInput GetInput()
            => new(GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRamdomBoolean()
        );

        public Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRamdomBoolean()
        );
        public bool getRamdomBoolean()
           => (new Random()).NextDouble() < 0.5;

    }
}
