using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.ListCategories
{
    public interface IListCategories 
        : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
    {
    }
}
