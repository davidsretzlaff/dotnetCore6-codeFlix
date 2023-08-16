using MediatR;
using MyFlix.Catalog.Application.UseCases.Genre.Common;

namespace MyFlix.Catalog.Application.UseCases.Genre.CreateGenre
{
    public class CreateGenreInput : IRequest<GenreModelOutput>
    {
        public CreateGenreInput(string name, bool isActive)
        {
            Name = name;
            IsActive = isActive;
        }

        public string Name { get; set; }
        public bool IsActive{ get; set; }
    }
}
