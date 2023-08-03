using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Category.DeleteCategory
{
    [CollectionDefinition(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTestFixtureCollection
        : ICollectionFixture<DeleteCategoryTestFixture>
    { }

    public class DeleteCategoryTestFixture
        : CategoryUseCasesBaseFixture
    {
    }
}