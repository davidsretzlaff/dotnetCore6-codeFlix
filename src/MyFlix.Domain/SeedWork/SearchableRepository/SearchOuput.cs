namespace CodeFlix.Catalog.Domain.SeedWork.SearchableRepository
{
    public class SearchOuput<TAggregate>
         where TAggregate : AggregateRoot
    {
        public SearchOuput(int currentPage, int perPage, int total, IReadOnlyList<TAggregate> items)
        {
            CurrentPage = currentPage;
            PerPage = perPage;
            Total = total;
            Items = items;
        }

        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public IReadOnlyList<TAggregate> Items { get; set; }
    }
}
