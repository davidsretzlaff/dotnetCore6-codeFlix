using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFlix.Catalog.Infra.Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.DeleteGenre
{
    [Collection(nameof(DeleteGenreTestApiFixture))]
    public class DeleteGenreTestApi : IDisposable
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

        [Fact(DisplayName = nameof(WhenNotFound404))]
        [Trait("EndToEnd/Api", "Genre/DeleteGenre - Endpoints")]
        public async Task WhenNotFound404()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var randomGuid = Guid.NewGuid();
            await _fixture.Persistence.InsertList(exampleGenres);

            var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>($"/genres/{randomGuid}");

            response.Should().NotBeNull();
            output.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output!.Type.Should().Be("NotFound");
            output.Detail.Should().Be($"Genre '{randomGuid}' not found.");
        }

        [Fact(DisplayName = nameof(DeleteGenreWithRelations))]
        [Trait("EndToEnd/Api", "Genre/DeleteGenre - Endpoints")]
        public async Task DeleteGenreWithRelations()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var targetGenre = exampleGenres[5];
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            Random random = new Random();
            exampleGenres.ForEach(genre =>
            {
                int relationsCount = random.Next(2, exampleCategories.Count - 1);
                for (int i = 0; i < relationsCount; i++)
                {
                    int selectedCategoryIndex = random.Next(0, exampleCategories.Count - 1);
                    var selected = exampleCategories[selectedCategoryIndex];
                    if (!genre.Categories.Contains(selected.Id))
                        genre.AddCategory(selected.Id);
                }
            });
            var genresCategories = new List<GenresCategories>();
            exampleGenres.ForEach(
                genre => genre.Categories.ToList().ForEach(
                    categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id))
                )
            );
            await _fixture.Persistence.InsertList(exampleGenres);
            await _fixture.CategoryPersistence.InsertList(exampleCategories);
            await _fixture.Persistence.InsertGenresCategoriesRelationsList(genresCategories);

            var (response, output) = await _fixture.ApiClient.Delete<object>($"/genres/{targetGenre.Id}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
            output.Should().BeNull();
            var genreDb = await _fixture.Persistence.GetById(targetGenre.Id);
            genreDb.Should().BeNull();
            List<GenresCategories> relations = await _fixture.Persistence.GetGenresCategoriesRelationsByGenreId(targetGenre.Id);
            relations.Should().HaveCount(0);
        }

		public void Dispose() => _fixture.CleanPersistence();
	}
}
