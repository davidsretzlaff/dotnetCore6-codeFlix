using Bogus;
using MyFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace MyFlix.Catalog.EndToEndTest.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }
        public ApiClient ApiClient { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
        }

        public CatalogDbContext CreateDbContext()
        {
            var context = new CatalogDbContext(
                new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase("end2end-tests-db")
                .Options
            );
            return context;
        }

    }
}
