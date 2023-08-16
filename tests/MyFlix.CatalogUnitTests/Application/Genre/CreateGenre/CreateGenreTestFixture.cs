using Moq;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.UnitTests.Application.Genre.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Genre.CreateGenre
{
    [CollectionDefinition(nameof(CreateGenreTestFixture))]
    public class CreateGenreTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture> { }
    public class CreateGenreTestFixture : GenreUseCasesBaseFixture
    {
        public CreateGenreInput GetExampleInput()
            => new CreateGenreInput(
                GetValidGenreName(),
                GetRandomBoolean()
            );
        public Mock<IGenreRepository> GetGenreRepositoryMock()
            => new();
        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new();
    }
}
