using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenres : IListGenres
    {
        private readonly IGenreRepository _genreRepository;
        public ListGenres(IGenreRepository genreRepository) => _genreRepository = genreRepository;
        
        public async Task<ListGenresOutput> Handle(ListGenresInput input, CancellationToken cancellationToken)
        {
            var searchOutput = await _genreRepository.Search(input.ToSearchInput(), cancellationToken);
            return ListGenresOutput.FromSearchOutput(searchOutput);
        }
    }
}
