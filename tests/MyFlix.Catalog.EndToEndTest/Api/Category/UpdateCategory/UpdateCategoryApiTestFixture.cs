using MyFlix.Catalog.Api.ApiModels.Category;
using MyFlix.Catalog.EndToEndTest.Api.Category.Common;
using System;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Category.UpdateCategory
{

    [CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTestFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture> { }

    public class UpdateCategoryApiTestFixture : CategoryBaseFixture
    {
        public UpdateCategoryApiInput GetExampleInput()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRandomBoolean()
            );
    }
}
