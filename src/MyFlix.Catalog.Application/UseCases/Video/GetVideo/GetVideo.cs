﻿using MyFlix.Catalog.Domain.Repository;

namespace MyFlix.Catalog.Application.UseCases.Video.GetVideo
{
	public class GetVideo : IGetVideo
	{
		private readonly IVideoRepository _repository;

		public GetVideo(IVideoRepository repository) => _repository = repository;

		public async Task<GetVideoOutput> Handle(GetVideoInput input, CancellationToken cancellationToken)
		{
			var video = await _repository.Get(input.VideoId, cancellationToken);
			return GetVideoOutput.FromVideo(video);
		}
	}
}
