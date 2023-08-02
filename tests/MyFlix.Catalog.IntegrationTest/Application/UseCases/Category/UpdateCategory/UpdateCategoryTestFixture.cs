﻿
using CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using CodeFlix.Catalog.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;

namespace CodeFlix.Catalog.IntegrationTest.Application.UseCases.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryTestFixture))]
    public class UpdateTextFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }
    public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public UpdateCategoryInput GetValidInput(Guid? id = null)
        {
            return new(
                   id ?? Guid.NewGuid(),
                   GetValidCategoryName(),
                   GetValidCategoryDescription(),
                   GetRandomBoolean()
               );
        }
        public UpdateCategoryInput GetInvalidInputShortName()
        {
            var input = GetValidInput();
            input.Name = input.Name.Substring(0, 2);
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
            input.Name = tooLongNameForCategory;
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
            input.Description = tooLongNameForDescription;
            return input;
        }
    }
}