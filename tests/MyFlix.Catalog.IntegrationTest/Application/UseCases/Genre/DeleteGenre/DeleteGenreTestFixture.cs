using MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.DeleteGenre
{
    [CollectionDefinition(nameof(DeleteGenreTestFixture))]
    public class DeleteGenreTestFixtureCollection : ICollectionFixture<DeleteGenreTestFixture> { }
    public class DeleteGenreTestFixture : GenreUseCasesBaseFixture
    {
    }
}
