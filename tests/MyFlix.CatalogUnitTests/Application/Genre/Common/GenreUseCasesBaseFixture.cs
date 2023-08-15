using MyFlix.Catalog.UnitTests.Common;

namespace MyFlix.Catalog.UnitTests.Application.Genre.Common
{
    public class GenreUseCasesBaseFixture : BaseFixture
    {
        public string GetValidGenreName()
        => Faker.Commerce.Categories(1)[0];
    }
}
