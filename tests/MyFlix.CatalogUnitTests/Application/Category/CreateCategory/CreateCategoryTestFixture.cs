using MyFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection :
        ICollectionFixture<CreateCategoryTestFixture>
    { }
    public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
    }
}
