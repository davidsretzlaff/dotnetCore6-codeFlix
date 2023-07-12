using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.ListCategories
{
    public interface IListCategories 
        : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
    {
    }
}
