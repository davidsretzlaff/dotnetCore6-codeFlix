using MyFlix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategoryInput : IRequest<CategoryModelOutput>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        public CreateCategoryInput(
            string name, 
            string? description = null, 
            bool isActive = true
            )
        {
            Name = name;
            Description = description ?? string.Empty;
            IsActive = isActive;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

		public void SetName(string name)
		{
			Name = name;
		}

		public void Active()
        {
			IsActive = true;
        }
        public void Desactive()
        {
            IsActive = false;
        }
    }
}
