using CodeFlix.Catalog.UnitTests.Application.Common;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetcagegoryTextFixtureCollection : ICollectionFixture<GetCategoryTestFixture> {}
    public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
    {
    }
}
