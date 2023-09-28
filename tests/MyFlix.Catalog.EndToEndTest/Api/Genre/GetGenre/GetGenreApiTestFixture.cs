using MyFlix.Catalog.EndToEndTest.Api.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.GetGenre
{
    [CollectionDefinition(nameof(GetGenreApiTestFixture))]
    public class GetGenreApiTestFixtureCollection : ICollectionFixture<GetGenreApiTestFixture> { }

    public class GetGenreApiTestFixture : GenreBaseFixture
    {
        public GetGenreApiTestFixture() : base()
        {
        }
    }
}
