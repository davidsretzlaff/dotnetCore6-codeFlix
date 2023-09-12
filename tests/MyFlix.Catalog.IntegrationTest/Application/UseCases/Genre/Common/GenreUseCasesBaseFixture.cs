using MyFlix.Catalog.IntegrationTest.Base;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.Common
{
    public class GenreUseCasesBaseFixture : BaseFixture
    {
        public string GetValidGenreName() 
            => Faker.Commerce.Categories(1)[0];

        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public DomainEntity.Genre GetExampleGenre(bool? isActive = null, List<Guid>? categoriesIds = null, string? name = null)
        {
            var genre = new DomainEntity.Genre(name ?? GetValidGenreName(), isActive ?? GetRandomBoolean());
            categoriesIds?.ForEach(genre.AddCategory);
            return genre;
        }

        public List<DomainEntity.Genre> GetExampleListGenres(int count = 10)
            => Enumerable.Range(1, count).Select(_ => GetExampleGenre()).ToList();
    }
}
