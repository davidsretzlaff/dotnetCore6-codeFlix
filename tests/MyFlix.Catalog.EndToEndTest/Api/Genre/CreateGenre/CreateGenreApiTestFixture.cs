using MyFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using MyFlix.Catalog.EndToEndTest.Api.Genre.Common;
using System;
using System.Collections.Generic;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.CreateGenre
{
    [CollectionDefinition(nameof(CreateGenreApiTestFixture))]
    public class CreateGenreApiTestFixtureCollection : ICollectionFixture<CreateGenreApiTestFixture> { }

    public class CreateGenreApiTestFixture : GenreBaseFixture
    {
        public CreateGenreInput GetCreateGenreInput(List<Guid> relatedCategories = null)
        {
            return new CreateGenreInput(
                GetValidCategoryName(),
                GetRandomBoolean(),
                relatedCategories ?? null
            ); ;
        }
    }
}