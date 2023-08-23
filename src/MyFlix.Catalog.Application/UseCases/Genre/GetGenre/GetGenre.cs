using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Catalog.Application.UseCases.Genre.GetGenre
{
    public class GetGenre : IGetGenre
    {
        private readonly IGenreRepository _genreRepository;

        public GetGenre(IGenreRepository genreRepository) => _genreRepository = genreRepository;

        public async Task<GenreModelOutput> Handle(GetGenreInput request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.Get(request.Id, cancellationToken);
            return GenreModelOutput.FromGenre(genre);
        }
    }
}
