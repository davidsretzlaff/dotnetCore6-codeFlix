using MyFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.CreateGenre
{
    [CollectionDefinition(nameof(CreateGenreTestFixture))]
    public class CreateGenreTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture>
    { }

    public class CreateGenreTestFixture : GenreUseCasesBaseFixture
    {
        public CreateGenreInput GetExampleInput()
            => new CreateGenreInput(GetValidGenreName(), GetRandomBoolean());
    }
}
