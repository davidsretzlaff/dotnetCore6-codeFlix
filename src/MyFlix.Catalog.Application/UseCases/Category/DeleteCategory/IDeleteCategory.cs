using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.DeleteCategory
{
    public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput>
    {}
}
