using MediatR;
using MyFlix.Catalog.Application.UseCases.Genre.Common;

namespace MyFlix.Catalog.Application.UseCases.Genre.CreateGenre
{
    public class CreateGenreInput : IRequest<GenreModelOutput>
    {
        public CreateGenreInput(string name, bool isActive, List<Guid>? categoriesIds = null)
        {
            Name = name;
            IsActive = isActive;
            CategoriesIds = categoriesIds;
        }

        public string Name { get; private set; }
        public bool IsActive{ get; private set; }
        public List<Guid>? CategoriesIds { get; private set; }

        public void setCategoriesIds(List<Guid> categoriesIds)
        {
            CategoriesIds = categoriesIds;
        }
    }
}
