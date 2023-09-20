using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.SeedWork;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Domain.Repository
{
    public interface ICategoryRepository : 
        IGenericRepository<Category>, 
        ISearchableRepository<Category>
    {
        public Task<IReadOnlyList<Guid>> GetIdsListByIds(List<Guid> ids, CancellationToken cancellationToken);
        public Task<IReadOnlyList<Category>> GetListByIds(List<Guid> ids, CancellationToken cancellationToken);
    }
}
