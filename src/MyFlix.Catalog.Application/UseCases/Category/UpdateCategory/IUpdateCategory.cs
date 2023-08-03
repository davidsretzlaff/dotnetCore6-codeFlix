using MyFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.UpdateCategory
{
    public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
    {

    }
}
