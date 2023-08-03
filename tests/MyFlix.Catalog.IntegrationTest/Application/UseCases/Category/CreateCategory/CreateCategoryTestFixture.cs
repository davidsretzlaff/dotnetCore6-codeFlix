using MyFlix.Catalog.Application.UseCases.Category.CreateCategory;
using MyFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using MyFlix.Catalog.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }
    public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public CreateCategoryInput GetInput()
        {
            var category = GetExampleCategory();
            return new CreateCategoryInput(
                category.Name,
                category.Description,
                category.IsActive
            );
        }
        public CreateCategoryInput GetValidInput(Guid? id = null)
        {
            return new(
                   GetValidCategoryName(),
                   GetValidCategoryDescription(),
                   GetRandomBoolean()
               );
        }
        public CreateCategoryInput GetInvalidInputShortName()
        {
            var input = GetValidInput();
            input.Name = input.Name.Substring(0, 2);
            return input;
        }
        public CreateCategoryInput GetInvalidInputTooLongName()
        {
            var input = GetValidInput();
            var tooLongNameForCategory = Faker.Commerce.ProductName();
            while (tooLongNameForCategory.Length <= 255)
            {
                tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
            }
            input.Name = tooLongNameForCategory;
            return input;
        }

        public CreateCategoryInput GetInvalidInputTooLongDescription()
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
