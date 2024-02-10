using MyFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategoryInput : IRequest<CategoryModelOutput>
    {
        public UpdateCategoryInput(Guid id, string name, string? description = null, bool? isActive = null)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public bool? IsActive { get; private set; }

        public void SetId(Guid id)
        {
            Id = id;
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}
