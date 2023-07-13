using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.UnitTests.Application.Common;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.DeleteCategory
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