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

        internal void FillWithCategoryNames(IReadOnlyList<DomainEntity.Category> categories)
        {
            foreach (GenreModelOutput item in Items)
                foreach (GenreModelOutputCategory categoryOutput in item.Categories)
                    categoryOutput.Name = categories.FirstOrDefault(
                        category => category.Id == categoryOutput.Id
                    )?.Name;
        }
    }
}
