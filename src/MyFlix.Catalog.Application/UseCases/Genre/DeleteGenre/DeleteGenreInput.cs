using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Genre.DeleteGenre
{
    public class DeleteGenreInput : IRequest
    {
        public DeleteGenreInput(Guid id) => Id = id;
        public Guid Id { get; private set; }
    }
}
