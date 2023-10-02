using MyFlix.Catalog.EndToEndTest.Api.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.DeleteGenre
{

    [CollectionDefinition(nameof(DeleteGenreTestApiFixture))]
    public class DeleteGenreTestApiFixtureCollection : ICollectionFixture<DeleteGenreTestApiFixture>{ }

    public class DeleteGenreTestApiFixture : GenreBaseFixture
    {
    }
}
