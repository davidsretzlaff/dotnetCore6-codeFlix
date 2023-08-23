using MediatR;
using MyFlix.Catalog.Application.UseCases.Genre.Common;

namespace MyFlix.Catalog.Application.UseCases.Genre.GetGenre
{
    public class GetGenreInput : IRequest<GenreModelOutput>
    {
        public GetGenreInput(Guid id) => Id = id;

        public Guid Id { get; set; }
    }
}
