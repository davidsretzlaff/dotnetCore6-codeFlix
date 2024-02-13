using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Infra.Data.EF.Repositories
{
	public class CastMemberRepository : ICastMemberRepository
	{
		private readonly CatalogDbContext _context;
		private DbSet<CastMember> _castMembers => _context.Set<CastMember>();

		public async Task Insert(CastMember aggregate, CancellationToken cancelationToken)
			=> await _castMembers.AddAsync(aggregate, cancelationToken);
		

		public CastMemberRepository(CatalogDbContext context) => _context = context;

		public Task Delete(CastMember aggregate, CancellationToken cancelationToken)
		{
			throw new NotImplementedException();
		}

		public Task<CastMember> Get(Guid id, CancellationToken cancelationToken)
		{
			throw new NotImplementedException();
		}

		public Task<SearchOutput<CastMember>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task Update(CastMember aggregate, CancellationToken cancelationToken)
		{
			throw new NotImplementedException();
		}
	}
}
