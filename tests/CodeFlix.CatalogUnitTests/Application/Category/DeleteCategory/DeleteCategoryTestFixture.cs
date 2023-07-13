using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.Category.DeleteCategory
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