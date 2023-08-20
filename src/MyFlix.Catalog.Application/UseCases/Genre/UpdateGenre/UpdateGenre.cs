using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.Domain.Repository;

namespace MyFlix.Catalog.Application.UseCases.Genre.UpdateGenre
{
    public class UpdateGenre : IUpdateGenre
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGenre(IGenreRepository genreRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            _genreRepository = genreRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<GenreModelOutput> Handle(UpdateGenreInput request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.Get(
                request.Id,
                cancellationToken
            );
            genre.Update(request.Name);
            if ( request.IsActive is not null && request.IsActive != genre.IsActive)
            {
                await ValidateCategoriesIds(request, cancellationToken);
                if ((bool)request.IsActive) genre.Activate();
                else genre.Deactivate();
            }
            if ((request.CategoriesIds?.Count ?? 0) > 0)
            {
                genre.RemoveAllCategories();
                request.CategoriesIds?.ForEach(genre.AddCategory);
            }
            await _genreRepository.Update(genre, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return GenreModelOutput.FromGenre(genre);
        }

        private async Task ValidateCategoriesIds(UpdateGenreInput request, CancellationToken cancellationToken)
        {
            var IdsInPersistence = await _categoryRepository.GetIdsListByIds(request.CategoriesIds!, cancellationToken);

            if (IdsInPersistence.Count < request.CategoriesIds!.Count)
            {
                var notFoundIds = request.CategoriesIds.FindAll(x => !IdsInPersistence.Contains(x));
                var notFoundIdsAsString = String.Join(", ", notFoundIds);
                
                throw new RelatedAggregateException( $"Related category id (or ids) not found: {notFoundIdsAsString}");
            }
        }
    }
}
