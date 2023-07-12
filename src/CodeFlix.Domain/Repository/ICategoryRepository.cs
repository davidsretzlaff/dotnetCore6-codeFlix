using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.SeedWork;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace CodeFlix.Catalog.Domain.Repository
{
    public interface ICategoryRepository : 
        IGenericRepository<Category>, 
        ISearchableRepository<Category>
    {
    }
}
