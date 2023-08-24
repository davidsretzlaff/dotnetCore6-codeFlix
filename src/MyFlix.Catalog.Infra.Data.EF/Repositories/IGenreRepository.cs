using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using MyFlix.Catalog.Domain.SeedWork;

namespace MyFlix.Catalog.Infra.Data.EF.Repositories
{
    public interface IGenreRepository : IGenericRepository<Genre>, ISearchableRepository<Genre>
    {
        
    }
}
