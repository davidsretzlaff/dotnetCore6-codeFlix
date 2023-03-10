using CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.Domain.SeedWork
{
    public interface IGenericRepository<TAgregate> : IRepository
    {
        public Task Insert(TAgregate agregate, CancellationToken cancelationToken);
    }
}
