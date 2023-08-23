using MyFlix.Catalog.UnitTests.Application.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Genre.GetGenre
{
    [CollectionDefinition(nameof(GetGenreTestFixture))]
    public class GetGenreTestFixtureCollection : ICollectionFixture<GetGenreTestFixture> { }

    public class GetGenreTestFixture: GenreUseCasesBaseFixture
    {
    }
}
