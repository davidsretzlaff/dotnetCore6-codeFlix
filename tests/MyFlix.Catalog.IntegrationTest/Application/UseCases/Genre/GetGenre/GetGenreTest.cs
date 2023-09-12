using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.GetGenre
{

    [Collection(nameof(GetGenreTestFixture))]
    public class GetGenreTest
    {
        private readonly GetGenreTestFixture _fixture;

        public GetGenreTest(GetGenreTestFixture fixture)
            => _fixture = fixture;
    }
}
