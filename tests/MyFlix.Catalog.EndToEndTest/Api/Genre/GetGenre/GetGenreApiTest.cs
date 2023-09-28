using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.GetGenre
{
    [Collection(nameof(GetGenreApiTestFixture))]
    public class GetGenreApiTest
    {
        private GetGenreApiTestFixture _fixture;

        public GetGenreApiTest(GetGenreApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(GetGenre))]
        [Trait("EndToEnd/API", "Genre/GetGenre - Endpoints")]
        public async Task GetGenre()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var targetGenre = exampleGenres[5];
            await _fixture.Persistence.InsertList(exampleGenres);

            var (response, output) = await _fixture.ApiClient.Get<ApiResponse<GenreModelOutput>>($"/genres/{targetGenre.Id}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Data.Id.Should().Be(targetGenre.Id);
            output.Data.Name.Should().Be(targetGenre.Name);
            output.Data.IsActive.Should().Be(targetGenre.IsActive);
        }
    }
}
