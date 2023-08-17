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

        public CreateGenreInput GetExampleInputWithCategories()
        {
            var numberOfCategoriesIds = (new Random()).Next(1, 10);
            var categoriesIds = Enumerable.Range(1, numberOfCategoriesIds)
                .Select(_ => Guid.NewGuid())
                .ToList();
            return new(
                GetValidGenreName(),
                GetRandomBoolean(),
                categoriesIds
            );
        }
        public Mock<IGenreRepository> GetGenreRepositoryMock()=> new();
        public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
        public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new();
    }
}
