using CodeFlix.Catalog.UnitTests.Application.Common;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection :
        ICollectionFixture<CreateCategoryTestFixture>
    { }
    public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
    }
}
