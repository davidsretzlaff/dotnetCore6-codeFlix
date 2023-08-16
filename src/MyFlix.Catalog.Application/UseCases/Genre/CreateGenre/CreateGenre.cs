using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.Domain.Repository;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Application.UseCases.Genre.CreateGenre
{
    public class CreateGenre
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGenre(
            IGenreRepository genreRepository,
            IUnitOfWork unitOfWork
        )
        {
            _genreRepository = genreRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenreModelOutput> Handle(
            CreateGenreInput request,
            CancellationToken cancellationToken
        )
        {
            var genre = new DomainEntity.Genre(
                request.Name,
                request.IsActive
            );
            if (request.CategoriesIds is not null)
                request.CategoriesIds.ForEach(genre.AddCategory);

            await _genreRepository.Insert(genre, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return GenreModelOutput.FromGenre(genre);
        }
    }
}
