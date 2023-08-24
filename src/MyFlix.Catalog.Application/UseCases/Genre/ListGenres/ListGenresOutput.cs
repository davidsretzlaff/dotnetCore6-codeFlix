using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenresOutput : PaginatedListOutput<GenreModelOutput>
    {
        public ListGenresOutput(int page, int perPage, int total, IReadOnlyList<GenreModelOutput> items) : base(page, perPage, total, items)
        {
        }
        public static ListGenresOutput FromSearchOutput(SearchOutput<DomainEntity.Genre> searchOutput) 
            => new(
                searchOutput.CurrentPage,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items
                    .Select(GenreModelOutput.FromGenre)
                    .ToList()
            );
    }
}
