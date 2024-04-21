using MyFlix.Catalog.Application.UseCases.Video.Common;
using MyFlix.Catalog.Domain.Repository;

namespace MyFlix.Catalog.Application.UseCases.Video.ListVideo
{
	public class ListVideos : IListVideos
	{
		private readonly IVideoRepository _videoRepository;

		public ListVideos(IVideoRepository videoRepository)
			=> _videoRepository = videoRepository;

		public async Task<ListVideosOutput> Handle(ListVideosInput input, CancellationToken cancellationToken)
		{
			var result = await _videoRepository.Search(input.ToSearchInput(), cancellationToken);
			return new ListVideosOutput(
				result.CurrentPage,
				result.PerPage,
				result.Total,
				result.Items.Select(VideoModelOutput.FromVideo).ToList());
		}
	}
}
