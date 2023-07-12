namespace CodeFlix.Catalog.Domain.SeedWork.SearchableRepository
{
    public interface ISearchableRepository<TAggregate> where TAggregate : AggregateRoot
    {
        Task<SearchOuput<TAggregate>> Search(
            SearchInput input,
            CancellationToken cancellationToken
        );
    }
}
