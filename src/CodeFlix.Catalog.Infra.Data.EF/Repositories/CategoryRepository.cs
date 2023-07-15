using CodeFlix.Catalog.Application.Exceptions;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.Infra.Data.EF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogDbContext _dbContext;
        private DbSet<Category> _categories => _dbContext.Set<Category>();

        public CategoryRepository(CatalogDbContext dbContext) => _dbContext = dbContext;

        public async Task Insert(Category aggregate, CancellationToken cancelationToken)
            => await _categories.AddAsync(aggregate, cancelationToken);

        public async Task<Category> Get(Guid id, CancellationToken cancelationToken)
        {
            var category =  await _categories.AsNoTracking().FirstOrDefaultAsync(
                x => x.Id == id, cancelationToken
            );
            NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");
            return category!;
        }

        public async Task Update(Category aggregate, CancellationToken cancelationToken)
            => Task.FromResult(_categories.Update(aggregate));
        

        public Task Delete(Category aggregate, CancellationToken cancelationToken)
        {
            throw new NotImplementedException();
        }

        public Task<SearchOuput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
