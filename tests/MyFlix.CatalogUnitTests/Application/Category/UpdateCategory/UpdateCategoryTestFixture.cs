using MyFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using MyFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTextFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }
    public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public UpdateCategoryInput GetValidInput(Guid? id = null)
        {
            return new(
                   id ?? Guid.NewGuid(),
                   GetValidCategoryName(),
                   GetValidCategoryDescription(),
                   getRamdomBoolean()
               );
        }
        public UpdateCategoryInput GetInvalidInputShortName()
        {
            var input = GetValidInput();
            input.SetName(input.Name.Substring(0, 2));
            return input;
        }
        public UpdateCategoryInput GetInvalidInputTooLongName()
        {
            var input = GetValidInput();
            var tooLongNameForCategory = Faker.Commerce.ProductName();
            while (tooLongNameForCategory.Length <= 255)
            {
                tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
            }
            input.SetName(tooLongNameForCategory);
            return input;
        }

        public UpdateCategoryInput GetInvalidInputTooLongDescription()
        {
            var input = GetValidInput();
            var tooLongNameForDescription = Faker.Commerce.ProductDescription();
            while (tooLongNameForDescription.Length <= 10_000)
            {
                tooLongNameForDescription = $"{tooLongNameForDescription} {Faker.Commerce.ProductDescription()}";
            }
            input.SetDescription(tooLongNameForDescription);
            return input;
        }
    }
}
