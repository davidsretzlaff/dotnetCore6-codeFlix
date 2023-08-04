using MyFlix.Catalog.EndToEndTest.Api.Category.Common;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Category.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryApiTestFixture))]
    public class DeleteCategoryApiTestFixtureCollection
    : ICollectionFixture<DeleteCategoryApiTestFixture>
    { }

    public class DeleteCategoryApiTestFixture
        : CategoryBaseFixture
    { }
}
