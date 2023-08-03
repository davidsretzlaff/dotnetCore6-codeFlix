using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategoriesOutput 
        : PaginatedListOutput<CategoryModelOutput>
    {
        public ListCategoriesOutput(
            int page, 
            int perPage, 
            int total, 
            IReadOnlyList<CategoryModelOutput> items) 
            : base(page, perPage, total, items)
        {
        }
    }
}
