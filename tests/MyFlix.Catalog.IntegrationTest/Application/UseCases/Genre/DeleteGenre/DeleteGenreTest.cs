﻿
using FluentAssertions;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.Infra.Data.EF;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.Genre.DeleteGenre;
using MyFlix.Catalog.Application.Exceptions;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.DeleteGenre
{
    [Collection(nameof(DeleteGenreTestFixture))]
    public class DeleteGenreTest 
    {
        private readonly DeleteGenreTestFixture _fixture;
        public DeleteGenreTest(DeleteGenreTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(DeleteGenre))]
        [Trait("Integration/Application", "DeleteGenre - Use Cases")]
        public async Task DeleteGenre()
        {
            var genresExampleList = _fixture.GetExampleListGenres(10);
            var targetGenre = genresExampleList[5];
            var dbArrangeContext = _fixture.CreateDbContext();
            await dbArrangeContext.Genres.AddRangeAsync(genresExampleList);
            await dbArrangeContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var useCase = new UseCase.DeleteGenre( new GenreRepository(actDbContext), new UnitOfWork(actDbContext));
            var input = new UseCase.DeleteGenreInput(targetGenre.Id);

            await useCase.Handle(input, CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);
            var genreFromDb = await assertDbContext.Genres.FindAsync(targetGenre.Id);
            genreFromDb.Should().BeNull();
        }

        [Fact(DisplayName = nameof(DeleteGenreThrowsWhenNotFound))]
        [Trait("Integration/Application", "DeleteGenre - Use Cases")]
        public async Task DeleteGenreThrowsWhenNotFound()
        {
            var genresExampleList = _fixture.GetExampleListGenres(10);
            var dbArrangeContext = _fixture.CreateDbContext();
            await dbArrangeContext.Genres.AddRangeAsync(genresExampleList);
            await dbArrangeContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var useCase = new UseCase.DeleteGenre(new GenreRepository(actDbContext), new UnitOfWork(actDbContext));
            var randomGuid = Guid.NewGuid();
            var input = new UseCase.DeleteGenreInput(randomGuid);

            Func<Task> action =async () => await useCase.Handle(input, CancellationToken.None);

            await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Genre '{randomGuid}' not found.");
        }
    }
}
