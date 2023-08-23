using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Genre.DeleteGenre
{
    public class IDeleteGenre : IRequestHandler<DeleteGenreInput>
    {
        public Task<Unit> Handle(DeleteGenreInput request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
