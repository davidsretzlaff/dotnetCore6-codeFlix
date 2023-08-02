using CodeFlix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.EndToEndTest.Api.Category.Common
{
    public class CategoryPersistence
    {
        private readonly CatalogDbContext _context;

        public CategoryPersistence(CatalogDbContext context)
            => _context = context;

        public async Task<DomainEntity.Category?> GetById(Guid id)
            => await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}
