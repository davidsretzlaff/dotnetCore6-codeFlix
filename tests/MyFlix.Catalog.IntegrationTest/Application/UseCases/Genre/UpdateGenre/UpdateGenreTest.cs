using FluentAssertions;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using UseCase = MyFlix.Catalog.Application.UseCases.Genre.UpdateGenre;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.Infra.Data.EF;
using Xunit;
using MyFlix.Catalog.Application.UseCases.Genre.UpdateGenre;
using MyFlix.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Application.Exceptions;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.UpdateGenre
{

    [Collection(nameof(UpdateGenreTestFixture))]
    public class UpdateGenreTest
    {
        private readonly UpdateGenreTestFixture _fixture;

        public UpdateGenreTest(UpdateGenreTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(UpdateGenre))]
        [Trait("Integration/Application", "UpdateGenre - Use Cases")]
        public async Task UpdateGenre()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var arrangeDbContext = _fixture.CreateDbContext();
            var targetGenre = exampleGenres[5];
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var updateGenre = new UseCase.UpdateGenre(
                new GenreRepository(actDbContext),
                new UnitOfWork(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var input = new UpdateGenreInput(
                targetGenre.Id,
                _fixture.GetValidGenreName(),
                !targetGenre.IsActive
            );

            GenreModelOutput output = await updateGenre.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(targetGenre.Id);
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be((bool)input.IsActive!);
            var assertDbContext = _fixture.CreateDbContext(true);
            var genreFromDb =await assertDbContext.Genres.FindAsync(targetGenre.Id);
            genreFromDb.Should().NotBeNull();
            genreFromDb!.Id.Should().Be(targetGenre.Id);
            genreFromDb.Name.Should().Be(input.Name);
            genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
        }

        [Fact(DisplayName = nameof(UpdateGenreWithCategoriesRelations))]
        [Trait("Integration/Application", "UpdateGenre - Use Cases")]
        public async Task UpdateGenreWithCategoriesRelations()
        {
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var arrangeDbContext = _fixture.CreateDbContext();
            var targetGenre = exampleGenres[5];
            var relatedCategories = exampleCategories.GetRange(0, 5);
            var newRelatedCategories = exampleCategories.GetRange(5, 3);
            relatedCategories.ForEach(category => targetGenre.AddCategory(category.Id));
            var relations = targetGenre.Categories.Select(categoryId => new GenresCategories(categoryId, targetGenre.Id)).ToList();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.AddRangeAsync(exampleCategories);
            await arrangeDbContext.AddRangeAsync(relations);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var updateGenre = new UseCase.UpdateGenre(
                new GenreRepository(actDbContext),
                new UnitOfWork(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var input = new UpdateGenreInput(
                targetGenre.Id,
                _fixture.GetValidGenreName(),
                !targetGenre.IsActive,
                newRelatedCategories.Select(category => category.Id).ToList()
            );

            GenreModelOutput output = await updateGenre.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(targetGenre.Id);
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be((bool)input.IsActive!);
            output.Categories.Should().HaveCount(newRelatedCategories.Count);
            var relatedCategoryIdsFromOutput =output.Categories.Select(relatedCategory => relatedCategory.Id).ToList();
            relatedCategoryIdsFromOutput.Should().BeEquivalentTo(input.CategoriesIds);
            var assertDbContext = _fixture.CreateDbContext(true);
            var genreFromDb = await assertDbContext.Genres.FindAsync(targetGenre.Id);
            genreFromDb.Should().NotBeNull();
            genreFromDb!.Id.Should().Be(targetGenre.Id);
            genreFromDb.Name.Should().Be(input.Name);
            genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
            var relatedcategoryIdsFromDb = await assertDbContext.GenresCategories.AsNoTracking()
                .Where(relation => relation.GenreId == input.Id)
                .Select(relation => relation.CategoryId)
                .ToListAsync();
            relatedcategoryIdsFromDb.Should().BeEquivalentTo(input.CategoriesIds);
        }

        [Fact(DisplayName = nameof(UpdateGenreThrowsWhenNotFound))]
        [Trait("Integration/Application", "UpdateGenre - Use Cases")]
        public async Task UpdateGenreThrowsWhenNotFound()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var arrangeDbContext = _fixture.CreateDbContext();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var updateGenre = new UseCase.UpdateGenre(
                new GenreRepository(actDbContext),
                new UnitOfWork(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var randomGuid = Guid.NewGuid();
            UpdateGenreInput input = new UpdateGenreInput(randomGuid, _fixture.GetValidGenreName(), true);

            var action = async () => await updateGenre.Handle(input, CancellationToken.None);

            await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{randomGuid}' not found.");
        }

        [Fact(DisplayName = nameof(UpdateGenreThrowsWhenCategoryDoesntExists))]
        [Trait("Integration/Application", "UpdateGenre - Use Cases")]
        public async Task UpdateGenreThrowsWhenCategoryDoesntExists()
        {
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var arrangeDbContext = _fixture.CreateDbContext();
            var targetGenre = exampleGenres[5];
            var relatedCategories = exampleCategories.GetRange(0, 5);
            var newRelatedCategories = exampleCategories.GetRange(5, 3);
            relatedCategories.ForEach(category => targetGenre.AddCategory(category.Id));
            var relations = targetGenre.Categories.Select(categoryId => new GenresCategories(categoryId, targetGenre.Id)).ToList();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.AddRangeAsync(exampleCategories);
            await arrangeDbContext.AddRangeAsync(relations);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            UseCase.UpdateGenre updateGenre = new UseCase.UpdateGenre(
                new GenreRepository(actDbContext),
                new UnitOfWork(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var categoryIdsToRelate = newRelatedCategories.Select(category => category.Id).ToList();
            var invalidCategoryId = Guid.NewGuid();
            categoryIdsToRelate.Add(invalidCategoryId);
            UpdateGenreInput input = new UpdateGenreInput(
                targetGenre.Id,
                _fixture.GetValidGenreName(),
                !targetGenre.IsActive,
                categoryIdsToRelate
            );

            var action = async () => await updateGenre.Handle(input,CancellationToken.None);

            await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related category id (or ids) not found: {invalidCategoryId}");
        }

        [Fact(DisplayName = nameof(UpdateGenreWithoutNewCategoriesRelations))]
        [Trait("Integration/Application", "UpdateGenre - Use Cases")]
        public async Task UpdateGenreWithoutNewCategoriesRelations()
        {
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var arrangeDbContext = _fixture.CreateDbContext();
            var targetGenre = exampleGenres[5];
            var relatedCategories = exampleCategories.GetRange(0, 5);
            relatedCategories.ForEach(category => targetGenre.AddCategory(category.Id));
            var relations = targetGenre.Categories.Select(categoryId => new GenresCategories(categoryId, targetGenre.Id)).ToList();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.AddRangeAsync(exampleCategories);
            await arrangeDbContext.AddRangeAsync(relations);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var updateGenre = new UseCase.UpdateGenre(
                new GenreRepository(actDbContext),
                new UnitOfWork(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var input = new UpdateGenreInput(targetGenre.Id, _fixture.GetValidGenreName(), !targetGenre.IsActive);

            GenreModelOutput output = await updateGenre.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(targetGenre.Id);
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be((bool)input.IsActive!);
            output.Categories.Should().HaveCount(relatedCategories.Count);
            var expectedRelatedCategoryIds = relatedCategories.Select(category => category.Id).ToList();
            var relatedCategoryIdsFromOutput = output.Categories.Select(relatedCategory => relatedCategory.Id).ToList();
            relatedCategoryIdsFromOutput.Should().BeEquivalentTo(expectedRelatedCategoryIds);
            var assertDbContext = _fixture.CreateDbContext(true);
            var genreFromDb = await assertDbContext.Genres.FindAsync(targetGenre.Id);
            genreFromDb.Should().NotBeNull();
            genreFromDb!.Id.Should().Be(targetGenre.Id);
            genreFromDb.Name.Should().Be(input.Name);
            genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
            var relatedcategoryIdsFromDb = await assertDbContext
                .GenresCategories.AsNoTracking()
                .Where(relation => relation.GenreId == input.Id)
                .Select(relation => relation.CategoryId)
                .ToListAsync();
            relatedcategoryIdsFromDb.Should().BeEquivalentTo(expectedRelatedCategoryIds);
        }

        [Fact(DisplayName = nameof(UpdateGenreWithEmptyCategoryIdsCleanRelations))]
        [Trait("Integration/Application", "UpdateGenre - Use Cases")]
        public async Task UpdateGenreWithEmptyCategoryIdsCleanRelations()
        {
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var arrangeDbContext = _fixture.CreateDbContext();
            var targetGenre = exampleGenres[5];
            var relatedCategories = exampleCategories.GetRange(0, 5);
            relatedCategories.ForEach(category => targetGenre.AddCategory(category.Id));
            List<GenresCategories> relations = targetGenre.Categories
                .Select(categoryId => new GenresCategories(categoryId, targetGenre.Id))
                .ToList();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.AddRangeAsync(exampleCategories);
            await arrangeDbContext.AddRangeAsync(relations);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            UseCase.UpdateGenre updateGenre = new UseCase.UpdateGenre(
                new GenreRepository(actDbContext),
                new UnitOfWork(actDbContext),
                new CategoryRepository(actDbContext)
            );
            UpdateGenreInput input = new UpdateGenreInput(
                targetGenre.Id,
                _fixture.GetValidGenreName(),
                !targetGenre.IsActive,
                new List<Guid>()
            );

            GenreModelOutput output = await updateGenre.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(targetGenre.Id);
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be((bool)input.IsActive!);
            output.Categories.Should().HaveCount(0);
            var relatedCategoryIdsFromOutput =
                output.Categories.Select(relatedCategory => relatedCategory.Id).ToList();
            relatedCategoryIdsFromOutput.Should().BeEquivalentTo(new List<Guid>());
            var assertDbContext = _fixture.CreateDbContext(true);
            var genreFromDb = await assertDbContext.Genres.FindAsync(targetGenre.Id);
            genreFromDb.Should().NotBeNull();
            genreFromDb!.Id.Should().Be(targetGenre.Id);
            genreFromDb.Name.Should().Be(input.Name);
            genreFromDb.IsActive.Should().Be((bool)input.IsActive!);
            var relatedcategoryIdsFromDb = await assertDbContext
                .GenresCategories.AsNoTracking()
                .Where(relation => relation.GenreId == input.Id)
                .Select(relation => relation.CategoryId)
                .ToListAsync();
            relatedcategoryIdsFromDb.Should().BeEquivalentTo(new List<Guid>());
        }
    }
}
