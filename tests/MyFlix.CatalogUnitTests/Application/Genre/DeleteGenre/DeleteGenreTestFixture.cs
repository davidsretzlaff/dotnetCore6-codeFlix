using MyFlix.Catalog.UnitTests.Application.Genre.Common;
using MyFlix.Catalog.UnitTests.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Genre.DeleteGenre
{
    [CollectionDefinition(nameof(DeleteGenreTestFixture))]
    public class DeleteGenreTestFixtureCollection : ICollectionFixture<DeleteGenreTestFixture> { }
    public class DeleteGenreTestFixture : GenreUseCasesBaseFixture
    {
    }
}
