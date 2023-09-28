using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.GetGenre
{
    [Collection(nameof(GetGenreApiTestFixture))]
    public class GetGenreApiTest
    {
        private GetGenreApiTestFixture _fixture;

        public GetGenreApiTest(GetGenreApiTestFixture fixture)
            => _fixture = fixture;
    }
}
