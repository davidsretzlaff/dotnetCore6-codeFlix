namespace MyFlix.Catalog.Domain.SeedWork
{
    public interface IGenericRepository<TAggregate> : IRepository
        where TAggregate : AggregateRoot
    {
        public Task Insert(TAggregate aggregate, CancellationToken cancelationToken);
        public Task<TAggregate> Get(Guid id, CancellationToken cancelationToken);
        public Task Delete(TAggregate aggregate, CancellationToken cancelationToken);
        public Task Update(TAggregate aggregate, CancellationToken cancelationToken);
    }
}
