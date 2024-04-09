
using MediatR;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Domain.Repository;

namespace MyFlix.Catalog.Application.UseCases.Video.DeleteVideo
{
	public class DeleteVideo : IDeleteVideo
	{
		private readonly IVideoRepository _repository;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteVideo(IVideoRepository repository, IUnitOfWork unitOfWork)
		{
			_repository = repository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(
			DeleteVideoInput input,
			CancellationToken cancellationToken)
		{
			var video = await _repository.Get(input.VideoId, cancellationToken);
			await _repository.Delete(video, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return Unit.Value;
		}
	}
}
