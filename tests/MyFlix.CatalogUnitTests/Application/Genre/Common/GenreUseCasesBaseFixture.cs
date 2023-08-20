using Moq;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.UnitTests.Common;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.UnitTests.Application.Genre.Common
{
    public class GenreUseCasesBaseFixture : BaseFixture
    {
        public Mock<IGenreRepository> GetGenreRepositoryMock() => new();
        public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();
        public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new();
        public string GetValidGenreName() => Faker.Commerce.Categories(1)[0];
        public DomainEntity.Genre GetExampleGenre(bool? isActive = null, List<Guid>? categoriesIds = null)
        {
            var genre = new DomainEntity.Genre(GetValidGenreName(), isActive ?? GetRandomBoolean());
            categoriesIds?.ForEach(genre.AddCategory);
            return genre;
        }

        public List<Guid> GetRandomIdsList(int? count = null)
            => Enumerable
                .Range(1, count ?? (new Random()).Next(1, 10))
                .Select(_ => Guid.NewGuid())
                .ToList();
    }
}
