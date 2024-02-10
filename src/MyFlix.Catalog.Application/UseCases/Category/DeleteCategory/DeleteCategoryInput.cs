using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategoryInput : IRequest
    {
        public DeleteCategoryInput(Guid id) => Id = id;

        public Guid Id { get; private set; }
    }
}
