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
		
		public Task<SearchOutput<CastMember>> Search(SearchInput input, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task Update(CastMember aggregate, CancellationToken cancelationToken)
			=> await Task.FromResult(_castMembers.Update(aggregate));	
	}
}
