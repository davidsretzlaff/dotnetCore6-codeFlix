using MyFlix.Catalog.EndToEndTest.Api.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.ListGenres
{

    [CollectionDefinition(nameof(ListGenresApiTestFixture))]
    public class ListGenresApiTestFixtureCiollection : ICollectionFixture<ListGenresApiTestFixture> 
    {
    }

    public class ListGenresApiTestFixture : GenreBaseFixture
    {
    }
}
