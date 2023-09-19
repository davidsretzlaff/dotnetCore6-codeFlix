using MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.ListGenres
{
    [CollectionDefinition(nameof(ListGenresTestFixture))]
    public class ListGenresTestFixtureCollection : ICollectionFixture<ListGenresTestFixture> { }
    public class ListGenresTestFixture : GenreUseCasesBaseFixture
    {
    }
}
