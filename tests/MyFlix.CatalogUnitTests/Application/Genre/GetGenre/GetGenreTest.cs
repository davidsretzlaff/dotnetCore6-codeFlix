using FluentAssertions;
using Moq;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.UnitTests.Domain.Entity.Genre;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Genre.GetGenre
{
    [Collection(nameof(GetGenreTestFixture))]
    public class GetGenreTest
    {
        readonly GetGenreTestFixture _fixture;

        public GetGenreTest(GetGenreTestFixture fixture) => _fixture = fixture;
        
        [Fact(DisplayName = nameof(GetGenre))]
        [Trait("Application", "GetGenre - Use Cases")]
        public async Task GetGenre()
        {
            var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
            var exampleGenre = _fixture.GetExampleGenre(
                categoriesIds: _fixture.GetRandomIdsList()
            );
            genreRepositoryMock.Setup(x => x.Get(
                It.Is<Guid>(x => x == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleGenre);
            var useCase = new UseCase.GetGenre(genreRepositoryMock.Object);
            var input = new UseCase.GetGenreInput(exampleGenre.Id);

            GenreModelOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleGenre.Id);
            output.Name.Should().Be(exampleGenre.Name);
            output.IsActive.Should().Be(exampleGenre.IsActive);
            output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
            output.Categories.Should().HaveCount(exampleGenre.Categories.Count);
            foreach (var expectedId in exampleGenre.Categories)
                output.Categories.Should().Contain(expectedId);
            genreRepositoryMock.Verify(
                x => x.Get(
                    It.Is<Guid>(x => x == exampleGenre.Id),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
