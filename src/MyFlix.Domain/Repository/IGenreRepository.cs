using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.SeedWork;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Domain.Repository
{
    public interface IGenreRepository : IGenericRepository<Genre>, ISearchableRepository<Genre>
    {
		public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids,CancellationToken cancellationToken);

		public Task<IReadOnlyList<Genre>> GetListByIds(List<Guid> ids, CancellationToken cancellationToken);
	}
}
