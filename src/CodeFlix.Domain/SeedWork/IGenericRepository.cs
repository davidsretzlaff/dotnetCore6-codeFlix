using CodeFlix.Catalog.Domain.Entity;

namespace CodeFlix.Catalog.Domain.SeedWork
{
    public interface IGenericRepository<TAgregate> : IRepository
    {
        public Task Insert(TAgregate agregate, CancellationToken cancelationToken);
        public Task<TAgregate> Get(Guid id, CancellationToken cancelationToken);
        public Task Delete(TAgregate agregate, CancellationToken cancelationToken);
        public Task Update(TAgregate agregate, CancellationToken cancelationToken);
    }
}
