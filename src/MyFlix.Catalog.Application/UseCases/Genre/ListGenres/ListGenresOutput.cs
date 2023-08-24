using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Application.UseCases.Genre.Common;

namespace MyFlix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenresOutput : PaginatedListOutput<GenreModelOutput>
    {
        public ListGenresOutput(int page, int perPage, int total, IReadOnlyList<GenreModelOutput> items) : base(page, perPage, total, items)
        {
        }
    }
}
