using MyFlix.Catalog.Infra.Data.EF;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.Common
{
    public class GenrePersistence
    {
        private readonly CatalogDbContext _context;

        public GenrePersistence(CatalogDbContext context) => _context = context;
    }
}
