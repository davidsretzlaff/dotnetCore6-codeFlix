using MyFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetcagegoryTextFixtureCollection : ICollectionFixture<GetCategoryTestFixture> { }
    public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
    {
    }
}
