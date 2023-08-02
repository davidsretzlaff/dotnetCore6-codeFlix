using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Domain.Repository;

namespace CodeFlix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategories : IListCategories
    {
        private readonly ICategoryRepository _categoryRepository;

        public ListCategories(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _categoryRepository.Search(
                new(
                    request.Page, 
                    request.PerPage, 
                    request.Search, 
                    request.Sort, 
                    request.Dir
               ), cancellationToken
            );
            var output = new ListCategoriesOutput(
                searchOutput.CurrentPage,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items
                .Select(CategoryModelOutput.FromCategory)
                .ToList()
            );
            return output;
            // line 31 is eq 
            //.Select(x => CategoryModelOutput.FromCategory(x))
        }
    }
}
