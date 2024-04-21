using MyFlix.Catalog.Application.UseCases.Video.Common;
using MyFlix.Catalog.Domain.Repository;
using DomainEntities = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Application.UseCases.Video.ListVideo
{
	public class ListVideos : IListVideos
	{
		private readonly IVideoRepository _videoRepository;
		private readonly ICategoryRepository _categoryRepository;

		public ListVideos(IVideoRepository videoRepository, ICategoryRepository categoryRepository)
		{
			_videoRepository = videoRepository;
			_categoryRepository = categoryRepository;
		}


		public async Task<ListVideosOutput> Handle(ListVideosInput input, CancellationToken cancellationToken)
		{
			var result = await _videoRepository.Search(input.ToSearchInput(), cancellationToken);
			IReadOnlyList<DomainEntities.Category>? categories = null;
			var relatedCategoriesIds = result.Items
				.SelectMany(video => video.Categories).Distinct().ToList();
			if (relatedCategoriesIds is not null && relatedCategoriesIds.Count > 0)
				categories = await _categoryRepository.GetListByIds(relatedCategoriesIds, cancellationToken);

			var output = new ListVideosOutput(
					result.CurrentPage,
				result.PerPage,
				result.Total,
				result.Items.Select(item => VideoModelOutput.FromVideo(item, categories)).ToList());
			return output;
		}
	}
}
