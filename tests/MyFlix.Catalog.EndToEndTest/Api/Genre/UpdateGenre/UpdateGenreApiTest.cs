﻿
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFlix.Catalog.Api.ApiModels.Genre;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.UpdateGenre
{
    [Collection(nameof(UpdateGenreApiTestFixture))]
    public class UpdateGenreApiTest
    {
        private readonly UpdateGenreApiTestFixture _fixture;

        public UpdateGenreApiTest(UpdateGenreApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(UpdateGenre))]
        [Trait("EndToEnd/Api", "Genre/UpdateGenre - Endpoints")]
        public async Task UpdateGenre()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var targetGenre = exampleGenres[5];
            await _fixture.Persistence.InsertList(exampleGenres);
            var input = new UpdateGenreApiInput(_fixture.GetValidGenreName(), _fixture.GetRandomBoolean());

            var (response, output) = await _fixture.ApiClient
                .Put<ApiResponse<GenreModelOutput>>(
                    $"/genres/{targetGenre.Id}",
                    input
                );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Data.Id.Should().Be(targetGenre.Id);
            output.Data.Name.Should().Be(input.Name);
            output.Data.IsActive.Should().Be((bool)input.IsActive!);
            var genreFromDb = await _fixture.Persistence.GetById(output.Data.Id);
            genreFromDb.Should().NotBeNull();
            genreFromDb!.Name.Should().Be(input.Name);
            genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
        }

        [Fact(DisplayName = nameof(ProblemDetailsWhenNotFound))]
        [Trait("EndToEnd/Api", "Genre/UpdateGenre - Endpoints")]
        public async Task ProblemDetailsWhenNotFound()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var randomGuid = Guid.NewGuid();
            await _fixture.Persistence.InsertList(exampleGenres);
            var input = new UpdateGenreApiInput(_fixture.GetValidGenreName(), _fixture.GetRandomBoolean());

            var (response, output) = await _fixture.ApiClient
                .Put<ProblemDetails>(
                    $"/genres/{randomGuid}",
                    input
                );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Title.Should().Be("Not Found");
            output!.Detail.Should().Be($"Genre '{randomGuid}' not found.");
            output!.Type.Should().Be("NotFound");
            output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        }
    }
}
