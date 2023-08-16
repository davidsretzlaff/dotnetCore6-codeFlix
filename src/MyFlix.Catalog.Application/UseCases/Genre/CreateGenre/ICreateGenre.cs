using MediatR;
using MyFlix.Catalog.Application.UseCases.Genre.Common;

namespace MyFlix.Catalog.Application.UseCases.Genre.CreateGenre
{
    public interface ICreateGenre
       : IRequestHandler<CreateGenreInput, GenreModelOutput>
    {
    }
}
