using Bogus;
using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.EndToEndTest.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }
        public ApiClient ApiClient { get; set; }

        public BaseFixture() => Faker = new Faker("pt_BR");

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
