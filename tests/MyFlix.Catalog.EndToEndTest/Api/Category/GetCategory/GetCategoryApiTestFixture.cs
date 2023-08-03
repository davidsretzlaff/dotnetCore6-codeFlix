
using MyFlix.Catalog.EndToEndTest.Api.Category.Common;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryApiTestFixture))]
    public class GetCategoryApiTestFixtureCollection
        : ICollectionFixture<GetCategoryApiTestFixture>
    { }

    public class GetCategoryApiTestFixture
        : CategoryBaseFixture
    { }
}
