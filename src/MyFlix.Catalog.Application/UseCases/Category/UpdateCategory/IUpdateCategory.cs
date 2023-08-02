using CodeFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory
{
    public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
    {

    }
}
