using MyFlix.Catalog.Infra.Data.EF;
using MyFlix.Catalog.Infra.Data.EF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.Common
{
    public class GenrePersistence
    {
        private readonly CatalogDbContext _context;

        public GenrePersistence(CatalogDbContext context) => _context = context;

        public async Task InsertList(List<DomainEntity.Genre> genres)
        {
            await _context.AddRangeAsync(genres);
            await _context.SaveChangesAsync();
        }
        public async Task InsertGenresCategoriesRelationsList(List<GenresCategories> relations)
        {
            await _context.AddRangeAsync(relations);
            await _context.SaveChangesAsync();
        }
    }
}
