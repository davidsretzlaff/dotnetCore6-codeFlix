﻿using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.IntegrationTest.Base;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.GenreRepository
{

    [CollectionDefinition(nameof(GenreRepositoryTestFixture))]
    public class GenreRepositoryTestFixtureCollection : ICollectionFixture<GenreRepositoryTestFixture> { }

    public class GenreRepositoryTestFixture : BaseFixture
    {
        public string GetValidGenreName() => Faker.Commerce.Categories(1)[0];

        public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

        public DomainEntity.Genre GetExampleGenre(bool? isActive = null, List<Guid>? categoriesIds = null, string? name = null)
        {
            var genre = new DomainEntity.Genre(name ?? GetValidGenreName(), isActive ?? GetRandomBoolean());
            categoriesIds?.ForEach(genre.AddCategory);
            return genre;
        }
        public List<DomainEntity.Genre> GetExampleListGenresByNames(List<string> names)
            => names.Select(name => GetExampleGenre(name: name)).ToList();

        public string GetValidCategoryName()
        {
            var categoryName = "";

            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];
            
            if (categoryName.Length > 255)
                categoryName = categoryName[..255];
            
            return categoryName;
        }
        public List<DomainEntity.Genre> GetExampleListGenres(int count = 10)
            => Enumerable.Range(1, count).Select(_ => GetExampleGenre()).ToList();

        public string GetValidCategoryDescription()
        {
            var categoryDescription = Faker.Commerce.ProductDescription();

            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];
            
            return categoryDescription;
        }

        public Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );

        public List<Category> GetExampleCategoriesList(int length = 10)
            => Enumerable.Range(1, length).Select(_ => GetExampleCategory()).ToList();
    }
}
