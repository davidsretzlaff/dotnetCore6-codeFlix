using CodeFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
    {}
}
