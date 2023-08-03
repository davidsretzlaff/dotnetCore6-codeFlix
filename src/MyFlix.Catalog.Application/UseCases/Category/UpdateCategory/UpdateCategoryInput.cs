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

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
