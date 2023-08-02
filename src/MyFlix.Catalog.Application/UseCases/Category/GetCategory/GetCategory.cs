using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Domain.Repository;

namespace CodeFlix.Catalog.Application.UseCases.Category.GetCategory
{
    public class GetCategory : IGetCategory
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategory(ICategoryRepository categoryRepository)
            => _categoryRepository = categoryRepository;

        public async Task<CategoryModelOutput> Handle(
            GetCategoryInput request, 
            CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.Id, cancellationToken);
            return CategoryModelOutput.FromCategory(category);
        }
    }
}
