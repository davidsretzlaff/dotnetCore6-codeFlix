using MediatR;
using MyFlix.Catalog.Application.UseCases.Genre.Common;

namespace MyFlix.Catalog.Application.UseCases.Genre.UpdateGenre
{
    public class UpdateGenreInput : IRequest<GenreModelOutput>
    {
        public UpdateGenreInput(Guid id, string name, bool? isActive = null, List<Guid>? categoriesIds = null)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            CategoriesIds = categoriesIds;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public List<Guid>? CategoriesIds { get; set; }
    }
}
