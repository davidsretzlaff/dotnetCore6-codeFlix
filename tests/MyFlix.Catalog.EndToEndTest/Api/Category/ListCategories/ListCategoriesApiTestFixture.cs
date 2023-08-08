using MyFlix.Catalog.EndToEndTest.Api.Category.Common;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Category.ListCategories
{

    [CollectionDefinition(nameof(ListCategoriesApiTestFixture))]
    public class ListCategoriesApiTestFixtureCollection
        : ICollectionFixture<ListCategoriesApiTestFixture>
    { }

    public class ListCategoriesApiTestFixture
        : CategoryBaseFixture
    { }
}
