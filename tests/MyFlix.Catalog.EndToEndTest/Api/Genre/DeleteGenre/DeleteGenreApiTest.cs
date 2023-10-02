using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.DeleteGenre
{
    [Collection(nameof(DeleteGenreTestApiFixture))]
    public class DeleteGenreTestApi
    {
        private readonly DeleteGenreTestApiFixture _fixture;

        public DeleteGenreTestApi(DeleteGenreTestApiFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(DeleteGenre))]
        [Trait("EndToEnd/Api", "Genre/DeleteGenre - Endpoints")]
        public async Task DeleteGenre()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var targetGenre = exampleGenres[5];
            await _fixture.Persistence.InsertList(exampleGenres);

            var (response, output) = await _fixture.ApiClient.Delete<object>($"/genres/{targetGenre.Id}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
            output.Should().BeNull();
            var genreDb = await _fixture.Persistence.GetById(targetGenre.Id);
            genreDb.Should().BeNull();
        }
    }
}
