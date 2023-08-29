﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Infra.Data.EF;
using MyFlix.Catalog.Infra.Data.EF.Models;
using Xunit;
using Repository = MyFlix.Catalog.Infra.Data.EF.Repositories;

namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.GenreRepository
{

    [Collection(nameof(GenreRepositoryTestFixture))]
    public class GenreRepositoryTest
    {
        private readonly GenreRepositoryTestFixture _fixture;

        public GenreRepositoryTest(GenreRepositoryTestFixture fixture)
            => _fixture = fixture;


        [Fact(DisplayName = nameof(Insert))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task Insert()
        {
            // Arrange 
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExample = _fixture.GetExampleCategoriesList(3);
            
            categoriesListExample.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            
            await dbContext.Categories.AddRangeAsync(categoriesListExample);
            var genreRepository = new Repository.GenreRepository(dbContext);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            // Act
            await genreRepository.Insert(exampleGenre, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            var assertsDbContext = _fixture.CreateDbContext(true);
            var dbGenre = await assertsDbContext.Genres.FindAsync(exampleGenre.Id);
            
            dbGenre.Should().NotBeNull();
            dbGenre!.Name.Should().Be(exampleGenre.Name);
            dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
            dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            
            var genreCategoriesRelations = await assertsDbContext
                .GenresCategories.Where(r => r.GenreId == exampleGenre.Id)
                .ToListAsync();
            
            genreCategoriesRelations.Should().HaveCount(categoriesListExample.Count);
            genreCategoriesRelations.ForEach(relation => 
            {
                var expectedCategory = categoriesListExample.FirstOrDefault(x => x.Id == relation.CategoryId);
                expectedCategory.Should().NotBeNull();
            });
        }

        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task Get()
        {
            // Arrange
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExample = _fixture.GetExampleCategoriesList(3);
            categoriesListExample.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            await dbContext.Categories.AddRangeAsync(categoriesListExample);
            await dbContext.Genres.AddAsync(exampleGenre);
            foreach (var categoryId in exampleGenre.Categories)
            {
                var relation = new GenresCategories(categoryId, exampleGenre.Id);
                await dbContext.GenresCategories.AddAsync(relation);
            }            
            dbContext.SaveChanges();
            
            // Act            
            var genreRepository = new Repository.GenreRepository(_fixture.CreateDbContext(true));
            var genreFromRepository = await genreRepository.Get(exampleGenre.Id, CancellationToken.None);
            
            // Assert
            genreFromRepository.Should().NotBeNull();
            genreFromRepository!.Name.Should().Be(exampleGenre.Name);
            genreFromRepository.IsActive.Should().Be(exampleGenre.IsActive);
            genreFromRepository.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            genreFromRepository.Categories.Should().HaveCount(categoriesListExample.Count);
            foreach (var categoryId in genreFromRepository.Categories)
            {
                var expectedCategory = categoriesListExample.FirstOrDefault(x => x.Id == categoryId);
                expectedCategory.Should().NotBeNull();
            };
        }

        [Fact(DisplayName = nameof(GetThrowWhenNotFound))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task GetThrowWhenNotFound()
        {
            // Arrange
            var exampleNotFoundGuid = Guid.NewGuid();
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExample = _fixture.GetExampleCategoriesList(3);
            categoriesListExample.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            await dbContext.Categories.AddRangeAsync(categoriesListExample);
            await dbContext.Genres.AddAsync(exampleGenre);
            foreach (var categoryId in exampleGenre.Categories)
            {
                var relation = new GenresCategories(categoryId, exampleGenre.Id);
                await dbContext.GenresCategories.AddAsync(relation);
            }
            dbContext.SaveChanges();

            // Act
            var genreRepository = new Repository.GenreRepository(_fixture.CreateDbContext(true));
            var action = async () => await genreRepository.Get(exampleNotFoundGuid, CancellationToken.None);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{exampleNotFoundGuid}' not found.");
        }

        [Fact(DisplayName = nameof(Delete))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task Delete()
        {
            // Arrange
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExample = _fixture.GetExampleCategoriesList(3);
            categoriesListExample.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            await dbContext.Categories.AddRangeAsync(categoriesListExample);
            await dbContext.Genres.AddAsync(exampleGenre);
            foreach (var categoryId in exampleGenre.Categories)
            {
                var relation = new GenresCategories(categoryId, exampleGenre.Id);
                await dbContext.GenresCategories.AddAsync(relation);
            }
            dbContext.SaveChanges();            
            var repositoryDbContext = _fixture.CreateDbContext(true);
            var genreRepository = new Repository.GenreRepository(repositoryDbContext);

            // Act
            await genreRepository.Delete(exampleGenre, CancellationToken.None);
            await repositoryDbContext.SaveChangesAsync();

            // Assert
            var assertsDbContext = _fixture.CreateDbContext(true);
            var dbGenre = assertsDbContext.Genres.AsNoTracking().FirstOrDefault(x => x.Id == exampleGenre.Id);
            dbGenre.Should().BeNull();
            var categoriesIdsList = await assertsDbContext.GenresCategories
                .AsNoTracking().Where(x => x.GenreId == exampleGenre.Id)
                .Select(x => x.CategoryId)
                .ToListAsync();
            categoriesIdsList.Should().HaveCount(0);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Integration/Infra.Data", "GenreRepository - Repositories")]
        public async Task Update()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleGenre = _fixture.GetExampleGenre();
            var categoriesListExample = _fixture.GetExampleCategoriesList(3);
            categoriesListExample.ForEach(
                category => exampleGenre.AddCategory(category.Id)
            );
            await dbContext.Categories.AddRangeAsync(categoriesListExample);
            await dbContext.Genres.AddAsync(exampleGenre);
            foreach (var categoryId in exampleGenre.Categories)
            {
                var relation = new GenresCategories(categoryId, exampleGenre.Id);
                await dbContext.GenresCategories.AddAsync(relation);
            }
            dbContext.SaveChanges();
            var genreRepository = new Repository.GenreRepository(_fixture.CreateDbContext(true));

            exampleGenre.Update(_fixture.GetValidGenreName());
            if (exampleGenre.IsActive)
                exampleGenre.Deactivate();
            else
                exampleGenre.Activate();
            await genreRepository.Update(exampleGenre, CancellationToken.None);

            var assertsDbContext = _fixture.CreateDbContext(true);
            var dbGenre = await assertsDbContext.Genres.FindAsync(exampleGenre.Id);
            dbGenre.Should().NotBeNull();
            dbGenre!.Name.Should().Be(exampleGenre.Name);
            dbGenre.IsActive.Should().Be(exampleGenre.IsActive);
            dbGenre.CreatedAt.Should().Be(exampleGenre.CreatedAt);
            var genreCategoriesRelations = await assertsDbContext
                .GenresCategories.Where(r => r.GenreId == exampleGenre.Id)
                .ToListAsync();
            genreCategoriesRelations.Should().HaveCount(categoriesListExample.Count);
            genreCategoriesRelations.ForEach(relation => {
                var expectedCategory = categoriesListExample.FirstOrDefault(x => x.Id == relation.CategoryId);
                expectedCategory.Should().NotBeNull();
            });
        }
    }
}
