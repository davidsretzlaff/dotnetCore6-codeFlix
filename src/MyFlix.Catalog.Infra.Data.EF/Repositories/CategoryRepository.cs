﻿using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace MyFlix.Catalog.Infra.Data.EF.Repositories
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

        public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
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

            return new SearchOutput<Category>(input.Page, input.PerPage, total, items);
        }

        private IQueryable<Category> AddOrderToQuery(
            IQueryable<Category> query,
            string orderProperty,
            SearchOrder order
        ) 
        {
            var orderedQuery = (orderProperty.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name).ThenBy(x => x.Id),
                ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
                ("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
                _ => query.OrderBy(x => x.Name).ThenBy(x => x.Id)
            };
            return orderedQuery;
        }

        public async Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellationToken)
        {
            return await _categories.AsNoTracking()
                .Where(category => ids.Contains(category.Id))
                .Select(category => category.Id).ToListAsync();
        }

        public async Task<IReadOnlyList<Category>> GetListByIds(List<Guid> ids, CancellationToken cancellationToken)
        {
            return await _categories.AsNoTracking()
               .Where(category => ids.Contains(category.Id))
               .ToListAsync();
        }
    }
}
