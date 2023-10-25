using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Genre.DeleteGenre
{
    public interface IDeleteGenre : IRequestHandler<DeleteGenreInput>
    {
    }

}
