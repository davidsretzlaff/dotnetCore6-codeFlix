using FluentAssertions;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using UseCase = MyFlix.Catalog.Application.UseCases.Genre.UpdateGenre;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.Infra.Data.EF;
using Xunit;

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
            UpdateGenreInput input = new UpdateGenreInput(
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
    }
}
