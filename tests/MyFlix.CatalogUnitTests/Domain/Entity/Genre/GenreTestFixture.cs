﻿using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.UnitTests.Common;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.UnitTests.Domain.Entity.Genre
{

    [CollectionDefinition(nameof(GenreTestFixture))]
    public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture> { }

    public class GenreTestFixture : BaseFixture
    {
        public string GetValidName() => Faker.Commerce.Categories(1)[0];
        public DomainEntity.Genre GetExampleGenre(bool isActive = true, List<Guid>? categoriesIdsList = null)
        {
            var genre = new DomainEntity.Genre(GetValidName(), isActive);
            if (categoriesIdsList is not null)
                foreach (var categoryId in categoriesIdsList)
                    genre.AddCategory(categoryId);
         
            return genre;
        }
    }
}
