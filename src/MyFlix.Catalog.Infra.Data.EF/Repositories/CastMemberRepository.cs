using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Infra.Data.EF.Repositories
{
	public class CastMemberRepository : ICastMemberRepository
	{
		private readonly CatalogDbContext _context;
		private DbSet<CastMember> _castMembers => _context.Set<CastMember>();
		public CastMemberRepository(CatalogDbContext context) => _context = context;

		public async Task Insert(CastMember aggregate, CancellationToken cancelationToken)
			=> await _castMembers.AddAsync(aggregate, cancelationToken);

		public async Task<CastMember> Get(Guid id, CancellationToken cancelationToken)
		{
			var castMember =  await _castMembers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancelationToken);
			NotFoundException.ThrowIfNull(castMember, $"CastMember '{id}' not found");
			return castMember!;
		}
			
		public async Task Delete(CastMember aggregate, CancellationToken cancelationToken)
			=> await Task.FromResult(_castMembers.Remove(aggregate));

		public async Task<SearchOutput<CastMember>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			var toSkip = (input.Page - 1) * input.PerPage;
			var query = _castMembers.AsNoTracking();
			query = AddOrderToQuery(query, input.OrderBy, input.Order);
			if (!String.IsNullOrWhiteSpace(input.Search))
				query = query.Where(x => x.Name.Contains(input.Search));
			var items = await query.Skip(toSkip).Take(input.PerPage).ToListAsync();
			var count = await query.CountAsync();
			return new SearchOutput<CastMember>(input.Page, input.PerPage, count, items.AsReadOnly());
		}

		public async Task Update(CastMember aggregate, CancellationToken cancelationToken)
			=> await Task.FromResult(_castMembers.Update(aggregate));

		private IQueryable<CastMember> AddOrderToQuery(IQueryable<CastMember> query, string orderProperty, SearchOrder order)
		{
			var orderedQuery = (orderProperty.ToLower(), order) switch
			{
				("name", SearchOrder.Asc) => query.OrderBy(x => x.Name)
					.ThenBy(x => x.Id),
				("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name)
					.ThenByDescending(x => x.Id),
				("id", SearchOrder.Asc) => query.OrderBy(x => x.Id),
				("id", SearchOrder.Desc) => query.OrderByDescending(x => x.Id),
				("createdat", SearchOrder.Asc) => query.OrderBy(x => x.CreatedAt),
				("createdat", SearchOrder.Desc) => query.OrderByDescending(x => x.CreatedAt),
				_ => query.OrderBy(x => x.Name)
					.ThenBy(x => x.Id)
			};
			return orderedQuery;
		}
	}
}
