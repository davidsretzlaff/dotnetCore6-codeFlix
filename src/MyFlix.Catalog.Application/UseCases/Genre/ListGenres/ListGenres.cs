using MyFlix.Catalog.Domain.Repository;

namespace MyFlix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenres : IListGenres
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ListGenres(IGenreRepository genreRepository, ICategoryRepository categoryRepository)
        => (_genreRepository, _categoryRepository) = (genreRepository, categoryRepository);

        public async Task<ListGenresOutput> Handle(ListGenresInput input, CancellationToken cancellationToken)
        {
            var searchOutput = await _genreRepository.Search(input.ToSearchInput(), cancellationToken);
            List<Guid> relatedCategoriesIds = searchOutput.Items
           .SelectMany(item => item.Categories)
           .Distinct()
           .ToList();
            var categories = await _categoryRepository.GetListByIds(relatedCategoriesIds, cancellationToken);
            ListGenresOutput output = ListGenresOutput.FromSearchOutput(searchOutput);
            output.FillWithCategoryNames(categories);
            return output;
        }
    }
}
