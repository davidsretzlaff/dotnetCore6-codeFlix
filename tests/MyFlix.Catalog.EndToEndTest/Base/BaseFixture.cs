using Bogus;
using MyFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;
using Microsoft.Extensions.Configuration;

namespace MyFlix.Catalog.EndToEndTest.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }
        public ApiClient ApiClient { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        private readonly string _dbConnectionString;
        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
            var configuration = WebAppFactory.Services
           .GetService(typeof(IConfiguration));
            ArgumentNullException.ThrowIfNull(configuration);
            _dbConnectionString = ((IConfiguration)configuration)
                .GetConnectionString("CatalogDb");
        }

        public CatalogDbContext CreateDbContext()
        {
            var context = new CatalogDbContext(
                new DbContextOptionsBuilder<CatalogDbContext>()
                 .UseMySql(
                    _dbConnectionString,
                    ServerVersion.AutoDetect(_dbConnectionString)
                )
                .Options
            );
            return context;
        }

        public void CleanPersistence()
        {
            var context = CreateDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
