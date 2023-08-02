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
            => Task.FromResult(_categories.Remove(aggregate));

        public async Task<SearchOuput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            var toSkip = (input.Page - 1) * input.PerPage;
            var query = _categories.AsNoTracking();
            query = AddOrderToQuery(query, input.OrderBy, input.Order);
           
            if (!string.IsNullOrWhiteSpace(input.Search))
            {
                query = query.Where(x => x.Name.Contains(input.Search));
            }

            var total = await query.CountAsync();
            var items = await query
                .Skip(toSkip)
                .Take(input.PerPage)
                .ToListAsync();

            return new SearchOuput<Category>(input.Page, input.PerPage, total, items);
        }

        private IQueryable<Category> AddOrderToQuery(
            IQueryable<Category> query,
            string orderProperty,
            SearchOrder order
        ) 
        {
            return (orderProperty.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderBy(x => x.Name)
            };
        }

    }
}
