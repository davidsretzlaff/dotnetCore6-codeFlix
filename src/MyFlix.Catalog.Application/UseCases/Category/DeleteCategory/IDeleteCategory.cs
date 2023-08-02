using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory
{
    public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput>
    {}
}
