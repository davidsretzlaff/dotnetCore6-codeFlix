using MediatR;
using MyFlix.Catalog.Application.UseCases.Genre.Common;

namespace MyFlix.Catalog.Application.UseCases.Genre.GetGenre
{
    public interface IGetGenre : IRequestHandler<GetGenreInput, GenreModelOutput>
    {
    }
}
