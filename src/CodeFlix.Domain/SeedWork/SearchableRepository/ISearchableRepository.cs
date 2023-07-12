namespace CodeFlix.Catalog.Domain.SeedWork.SearchableRepository
{
    public interface ISearchableRepository<Taggregate> where Taggregate : AggregateRoot
    {
        Task<SearchOuput<Taggregate>> Search(
            SearchInput input,
            CancellationToken cancellationToken
        );
    }
}
