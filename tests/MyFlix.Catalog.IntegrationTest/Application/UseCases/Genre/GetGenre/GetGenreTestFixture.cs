using MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.GetGenre
{

    [CollectionDefinition(nameof(GetGenreTestFixture))]    
    public class GetGenreTestFixtureCollection : ICollectionFixture<GetGenreTestFixture>
    { 
    }
    public class GetGenreTestFixture : GenreUseCasesBaseFixture
    {
    }
}
