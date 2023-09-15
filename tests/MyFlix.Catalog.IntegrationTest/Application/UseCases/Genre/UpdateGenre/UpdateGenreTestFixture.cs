using MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.UpdateGenre
{
    [CollectionDefinition(nameof(UpdateGenreTestFixture))]
    public class UpdateGenreTestFixtureCollection : ICollectionFixture<UpdateGenreTestFixture>
    {
    }

    public class UpdateGenreTestFixture : GenreUseCasesBaseFixture
    { 
    }
}
