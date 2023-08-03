using MyFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
    {}
}
