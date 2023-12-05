﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.Application.UseCases.Genre.ListGenres;
using MyFlix.Catalog.EndToEndTest.Extensions.DataTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.ListGenres
{

    [Collection(nameof(ListGenresApiTestFixture))]
	public class ListGenresApiTest : IDisposable
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

            var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<GenreModelOutput>>("/genres", input);

			response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
			output!.Meta.Should().NotBeNull();
			output.Data.Should().NotBeNull();
			output.Meta!.Total.Should().Be(exampleGenres.Count);
			output.Meta.CurrentPage.Should().Be(input.Page);
			output.Meta.PerPage.Should().Be(input.PerPage);
			output.Data!.Count.Should().Be(exampleGenres.Count);
			output.Data.ToList().ForEach(outputItem =>
			{
                var exampleItem = exampleGenres.Find(x => x.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            });
        }

		[Fact(DisplayName = nameof(EmptyWhenThereAreNoItems))]
		[Trait("EndToEnd/Api", "Genre/ListGenres - Endpoints")]
		public async Task EmptyWhenThereAreNoItems()
		{
			var input = new ListGenresInput();
			input.Page = 1;
			input.PerPage = 15;

			var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<GenreModelOutput>>("/genres", input);

			response.Should().NotBeNull();
			response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
			output.Should().NotBeNull();
			output!.Meta.Should().NotBeNull();
			output.Data.Should().NotBeNull();
			output.Meta!.Total.Should().Be(0);
			output.Data!.Count.Should().Be(0);
		}

		[Theory(DisplayName = nameof(ListPaginated))]
		[Trait("EndToEnd/Api", "Genre/ListGenres - Endpoints")]
		[InlineData(10, 1, 5, 5)]
		[InlineData(10, 2, 5, 5)]
		[InlineData(7, 2, 5, 2)]
		[InlineData(7, 3, 5, 0)]
		public async Task ListPaginated(int quantityToGenerate, int page, int perPage, int expectedQuantityItems)
		{
			var exampleGenres = _fixture.GetExampleListGenres(quantityToGenerate);
			await _fixture.Persistence.InsertList(exampleGenres);
			var input = new ListGenresInput();
			input.Page = page;
			input.PerPage = perPage;

			var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<GenreModelOutput>>("/genres", input);

			response.Should().NotBeNull();
			response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
			output.Should().NotBeNull();
			output!.Meta.Should().NotBeNull();
			output.Data.Should().NotBeNull();
			output.Meta!.Total.Should().Be(quantityToGenerate);
			output.Meta.CurrentPage.Should().Be(input.Page);
			output.Meta.PerPage.Should().Be(input.PerPage);
			output.Data!.Count.Should().Be(expectedQuantityItems);
			output.Data.ToList().ForEach(outputItem =>
			{
				var exampleItem = exampleGenres.Find(x => x.Id == outputItem.Id);
				exampleItem.Should().NotBeNull();
				outputItem.Name.Should().Be(exampleItem!.Name);
				outputItem.IsActive.Should().Be(exampleItem.IsActive);
				outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
			});
		}

		[Theory(DisplayName = nameof(SearchByText))]
		[Trait("EndToEnd/Api", "Genre/ListGenres - Endpoints")]
		[InlineData("Action", 1, 5, 1, 1)]
		[InlineData("Horror", 1, 5, 3, 3)]
		[InlineData("Horror", 2, 5, 0, 3)]
		[InlineData("Sci-fi", 1, 5, 4, 4)]
		[InlineData("Sci-fi", 1, 2, 2, 4)]
		[InlineData("Sci-fi", 2, 3, 1, 4)]
		[InlineData("Sci-fi Other", 1, 3, 0, 0)]
		[InlineData("Robots", 1, 5, 2, 2)]
		public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
		{
			var exampleGenres = _fixture.GetExampleListGenresByNames(
				new List<string>() {
				"Action",
				"Horror",
				"Horror - Robots",
				"Horror - Based on Real Facts",
				"Drama",
				"Sci-fi IA",s
				"Sci-fi Space",
				"Sci-fi Robots",
				"Sci-fi Future"
				}
			);

			await _fixture.Persistence.InsertList(exampleGenres);
			var input = new ListGenresInput();
			input.Page = page;
			input.PerPage = perPage;
			input.Search = search;

			var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<GenreModelOutput>>("/genres", input);

			response.Should().NotBeNull();
			response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
			output.Should().NotBeNull();
			output!.Meta.Should().NotBeNull();
			output.Data.Should().NotBeNull();
			output.Meta!.Total.Should().Be(expectedQuantityTotalItems);
			output.Meta.CurrentPage.Should().Be(input.Page);
			output.Meta.PerPage.Should().Be(input.PerPage);
			output.Data!.Count.Should().Be(expectedQuantityItemsReturned);
			output.Data.ToList().ForEach(outputItem =>
			{
				var exampleItem = exampleGenres.Find(x => x.Id == outputItem.Id);
				exampleItem.Should().NotBeNull();
				outputItem.Name.Should().Be(exampleItem!.Name);
				outputItem.IsActive.Should().Be(exampleItem.IsActive);
				outputItem.CreatedAt.TrimMillisseconds().Should().Be(exampleItem.CreatedAt.TrimMillisseconds());
			});
		}


		public void Dispose() => _fixture.CleanPersistence();
	}
}