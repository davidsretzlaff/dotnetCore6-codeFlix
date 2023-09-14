using FluentAssertions;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using UseCase = MyFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.Infra.Data.EF;
using Xunit;

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
    }
}
