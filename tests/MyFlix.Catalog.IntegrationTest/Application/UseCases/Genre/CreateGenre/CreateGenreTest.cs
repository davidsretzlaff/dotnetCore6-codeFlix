using FluentAssertions;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using UseCase = MyFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.Infra.Data.EF;
using Xunit;
using MyFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using MyFlix.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.CreateGenre
{
    [Collection(nameof(CreateGenreTestFixture))]
    public class CreateGenreTest
    {
        private readonly CreateGenreTestFixture _fixture;

        public CreateGenreTest(CreateGenreTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateGenre))]
        [Trait("Integration/Application", "CreateGenre - Use Cases")]
        public async Task CreateGenre()
        {
            var input = _fixture.GetExampleInput();
            var actDbContext = _fixture.CreateDbContext();
            var createGenre = new UseCase.CreateGenre(
                new GenreRepository(actDbContext),
                new UnitOfWork(actDbContext),
                new CategoryRepository(actDbContext)
            );

            GenreModelOutput output = await createGenre.Handle(input, CancellationToken.None);

            output.Id.Should().NotBeEmpty();
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be(input.IsActive);
            output.CreatedAt.Should().NotBe(default(DateTime));
            output.Categories.Should().HaveCount(0);
            var assertDbContext = _fixture.CreateDbContext(true);
            var genreFromDb = await assertDbContext.Genres.FindAsync(output.Id);
            genreFromDb.Should().NotBeNull();
            genreFromDb!.Name.Should().Be(input.Name);
            genreFromDb.IsActive.Should().Be(input.IsActive);
        }

        [Fact(DisplayName = nameof(CreateGenreWithCategoriesRelations))]
        [Trait("Integration/Application", "CreateGenre - Use Cases")]
        public async Task CreateGenreWithCategoriesRelations()
        {
            var exampleCategories = _fixture.GetExampleCategoriesList(5);
            var arrangeDbContext = _fixture.CreateDbContext();
            await arrangeDbContext.Categories.AddRangeAsync(exampleCategories);
            await arrangeDbContext.SaveChangesAsync();
            CreateGenreInput input = _fixture.GetExampleInput();
            input.CategoriesIds = exampleCategories.Select(category => category.Id).ToList();
            var actDbContext = _fixture.CreateDbContext(true);
            UseCase.CreateGenre createGenre = new UseCase.CreateGenre(
                new GenreRepository(actDbContext),
                new UnitOfWork(actDbContext),
                new CategoryRepository(actDbContext)
            );

            GenreModelOutput output = await createGenre.Handle(input, CancellationToken.None);

            output.Id.Should().NotBeEmpty();
            output.Name.Should().Be(input.Name);
            output.IsActive.Should().Be(input.IsActive);
            output.CreatedAt.Should().NotBe(default(DateTime));
            output.Categories.Should().HaveCount(input.CategoriesIds.Count);
            var relatedCategoriesIdsFromOutput = output.Categories.Select(relation => relation.Id).ToList();
            relatedCategoriesIdsFromOutput.Should().BeEquivalentTo(input.CategoriesIds);
            var assertDbContext = _fixture.CreateDbContext(true);
            var genreFromDb = await assertDbContext.Genres.FindAsync(output.Id);
            genreFromDb.Should().NotBeNull();
            genreFromDb!.Name.Should().Be(input.Name);
            genreFromDb.IsActive.Should().Be(input.IsActive);
            var relations = await assertDbContext.GenresCategories.AsNoTracking()
                    .Where(x => x.GenreId == output.Id)
                    .ToListAsync();
            relations.Should().HaveCount(input.CategoriesIds.Count);
            var categoryIdsRelatedFromDb = relations.Select(relation => relation.CategoryId).ToList();
            categoryIdsRelatedFromDb.Should().BeEquivalentTo(input.CategoriesIds);
        }
    }
}
