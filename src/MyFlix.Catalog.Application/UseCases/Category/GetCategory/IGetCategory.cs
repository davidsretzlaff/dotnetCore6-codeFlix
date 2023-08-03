using MyFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.GetCategory
{
    public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
    {

    }
}
