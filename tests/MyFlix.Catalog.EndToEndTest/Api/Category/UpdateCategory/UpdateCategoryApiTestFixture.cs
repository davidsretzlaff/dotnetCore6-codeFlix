using MyFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using MyFlix.Catalog.EndToEndTest.Api.Category.Common;
using System;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Category.UpdateCategory
{

    [CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTestFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture> { }

    public class UpdateCategoryApiTestFixture : CategoryBaseFixture
    {
        public UpdateCategoryInput GetExampleInput(Guid? id = null)
            => new(
                id ?? Guid.NewGuid(),
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRandomBoolean()
            );
    }
}
