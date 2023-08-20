using MediatR;
using MyFlix.Catalog.Application.UseCases.Genre.Common;

namespace MyFlix.Catalog.Application.UseCases.Genre.UpdateGenre
{
    public interface IUpdateGenre :IRequestHandler<UpdateGenreInput,GenreModelOutput>
    {
    }
}
