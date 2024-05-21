using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.Video.Common;
using MyFlix.Catalog.Domain.Exceptions;
using MyFlix.Catalog.Domain.Repository;
using DomainEntities = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Application.UseCases.Video.UpdateVideo
{
	public class UpdateVideo : IUpdateVideo
	{
		private readonly IVideoRepository _videoRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateVideo(IVideoRepository videoRepository, IUnitOfWork unitOfWork)
		{
			_videoRepository = videoRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<VideoModelOutput> Handle(
			UpdateVideoInput input,
			CancellationToken cancellationToken)
		{
			var video = await _videoRepository.Get(input.VideoId, cancellationToken);
			video.Update(
				input.Title,
				input.Description,
				input.YearLaunched,
				input.Opened,
				input.Published,
				input.Duration,
				input.Rating);
			await _videoRepository.Update(video, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);
			return VideoModelOutput.FromVideo(video);
		}
	}
}
