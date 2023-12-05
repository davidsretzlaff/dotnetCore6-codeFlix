﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.Genre.ListGenres;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.ListGenres
{

    [Collection(nameof(ListGenresApiTestFixture))]
    public class ListGenresApiTest
    {
        private readonly ListGenresApiTestFixture _fixture;

        public ListGenresApiTest(ListGenresApiTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(List))]
        [Trait("EndToEnd/Api", "Genre/ListGenres - Endpoints")]
        public async Task List()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            await _fixture.Persistence.InsertList(exampleGenres);
            var input = new ListGenresInput();
            input.Page = 1;
            input.PerPage = exampleGenres.Count;

            var (response, output) = await _fixture.ApiClient.Get<ApiResponse<ListGenresOutput>>("/genres", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output!.Data.Total.Should().Be(exampleGenres.Count);
            output.Data.Items.Count.Should().Be(exampleGenres.Count);
            output.Data.Page.Should().Be(input.Page);
            output.Data.PerPage.Should().Be(input.PerPage);
            output.Data.Items.ToList().ForEach(outputItem =>
            {
                var exampleItem = exampleGenres.Find(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            });
        }
    }
}