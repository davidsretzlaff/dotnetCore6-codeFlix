
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFlix.Catalog.Api.ApiModels.Genre;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.Infra.Data.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact(DisplayName = nameof(UpdateGenreWithRelations))]
        [Trait("EndToEnd/Api", "Genre/UpdateGenre - Endpoints")]
        public async Task UpdateGenreWithRelations()
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
                    categoryId => genresCategories.Add(
                        new GenresCategories(categoryId, genre.Id)
                    )
                )
            );
            int newRelationsCount = random.Next(2, exampleCategories.Count - 1);
            var newRelatedCategoriesIds = new List<Guid>();
            for (int i = 0; i < newRelationsCount; i++)
            {
                int selectedCategoryIndex = random.Next(0, exampleCategories.Count - 1);
                var selected = exampleCategories[selectedCategoryIndex];
                if (!newRelatedCategoriesIds.Contains(selected.Id))
                    newRelatedCategoriesIds.Add(selected.Id);
            }
            await _fixture.Persistence.InsertList(exampleGenres);
            await _fixture.CategoryPersistence.InsertList(exampleCategories);
            await _fixture.Persistence.InsertGenresCategoriesRelationsList(genresCategories);
            var input = new UpdateGenreApiInput(
                _fixture.GetValidGenreName(),
                _fixture.GetRandomBoolean(),
                newRelatedCategoriesIds
            );

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
            var relatedCategoriesIdsFromOutput = output.Data.Categories.Select(relation => relation.Id).ToList();
            relatedCategoriesIdsFromOutput.Should().BeEquivalentTo(newRelatedCategoriesIds);
            var genreFromDb = await _fixture.Persistence.GetById(output.Data.Id);
            genreFromDb.Should().NotBeNull();
            genreFromDb!.Name.Should().Be(input.Name);
            genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
            var genresCategoriesFromDb = await _fixture.Persistence.GetGenresCategoriesRelationsByGenreId(targetGenre.Id);
            var relatedCategoriesIdsFromDb = genresCategoriesFromDb.Select(x => x.CategoryId).ToList();
            relatedCategoriesIdsFromDb.Should().BeEquivalentTo(newRelatedCategoriesIds);
        }
    }
}
