using CodeFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Category.GetCategory
{
    public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
    {

    }
}
