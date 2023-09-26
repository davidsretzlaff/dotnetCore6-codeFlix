using MyFlix.Catalog.Domain.Repository;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

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
            var output = ListGenresOutput.FromSearchOutput(searchOutput);

            var relatedCategoriesIds = searchOutput.Items.SelectMany(item => item.Categories).Distinct().ToList();

            if (relatedCategoriesIds.Count > 0)
            {
                var categories = await _categoryRepository.GetListByIds(relatedCategoriesIds, cancellationToken);
                output.FillWithCategoryNames(categories);
            }
            return output;
        }
    }
}
