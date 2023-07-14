using Bogus;

namespace CodeFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.Base
{
    public class BaseFixture
    {
        public BaseFixture() => Faker = new Faker("pt_BR");

        protected Faker Faker { get; set; }

    }
}
