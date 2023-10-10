
using MyFlix.Catalog.EndToEndTest.Api.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.UpdateGenre
{

    [CollectionDefinition(nameof(UpdateGenreApiTestFixture))]
    public class UpdateGenreApiTestFixtureCollection : ICollectionFixture<UpdateGenreApiTestFixture> { } 

    public class UpdateGenreApiTestFixture : GenreBaseFixture { }
}
