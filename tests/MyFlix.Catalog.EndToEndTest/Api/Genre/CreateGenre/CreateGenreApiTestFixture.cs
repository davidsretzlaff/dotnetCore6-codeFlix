using MyFlix.Catalog.EndToEndTest.Api.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.CreateGenre
{
    [CollectionDefinition(nameof(CreateGenreApiTestFixture))]
    public class CreateGenreApiTestFixtureCollection : ICollectionFixture<CreateGenreApiTestFixture> { }

    public class CreateGenreApiTestFixture : GenreBaseFixture
    {
    }
}
