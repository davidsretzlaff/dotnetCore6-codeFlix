using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTextFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }
    public class UpdateCategoryTestFixture : BaseFixture
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
        public bool getRamdomBoolean()
         => (new Random()).NextDouble() < 0.5;

        public Category GetCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRamdomBoolean()
            );

        public UpdateCategoryInput GetValidInput(Guid? id = null)
        {
            return new (
                   id ?? Guid.NewGuid(),
                   GetValidCategoryName(),
                   GetValidCategoryDescription(),
                   getRamdomBoolean()
               );
        }
        public UpdateCategoryInput GetInvalidInputShortName()
        {
            var input = GetValidInput();
            input.Name = input.Name.Substring(0,2);
            return input;
        }
        public UpdateCategoryInput GetInvalidInputTooLongName()
        {
            var input = GetValidInput();
            var tooLongNameForCategory = Faker.Commerce.ProductName();
            while(tooLongNameForCategory.Length <= 255)
            {
                tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
            }
            input.Name = tooLongNameForCategory;
            return input;
        }

        //public UpdateCategoryInput GetInvalidInputCategoryNull
        public UpdateCategoryInput GetInvalidInputTooLongDescription()
        {
            var input = GetValidInput();
            var tooLongNameForDescription = Faker.Commerce.ProductDescription();
            while (tooLongNameForDescription.Length <= 10_000)
            {
                tooLongNameForDescription = $"{tooLongNameForDescription} {Faker.Commerce.ProductDescription()}";
            }
            input.Description = tooLongNameForDescription;
            return input;
        }
    }
}
