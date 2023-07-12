using CodeFlix.Catalog.Application.Common;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategoriesInput 
        : PaginatedListInput, IRequest<ListCategoriesOutput>
    {
        public ListCategoriesInput(
            int page, 
            int perPage, 
            string search, 
            string sort, 
            SearchOrder dir) 
            : base(page, perPage, search, sort, dir)
        {
        }
    }
}
