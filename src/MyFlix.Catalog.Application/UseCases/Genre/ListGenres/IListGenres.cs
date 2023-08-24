using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Genre.ListGenres
{
    public interface IListGenres : IRequestHandler<ListGenresInput, ListGenresOutput>
    {
    }
}
