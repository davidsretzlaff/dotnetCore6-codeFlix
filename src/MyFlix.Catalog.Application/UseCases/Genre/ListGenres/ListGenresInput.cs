using MediatR;
using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenresInput : PaginatedListInput, IRequest<ListGenresOutput>
    {
        public ListGenresInput(
            int page = 1,
            int perPage = 15,
            string search = "",
            string sort = "",
            SearchOrder dir = SearchOrder.Asc
        )
        : base(page, perPage, search, sort, dir)
        { }
    }
}
