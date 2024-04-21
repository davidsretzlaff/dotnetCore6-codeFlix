
using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Application.UseCases.Video.Common;

namespace MyFlix.Catalog.Application.UseCases.Video.ListVideo
{
	public class ListVideosOutput : PaginatedListOutput<VideoModelOutput>
	{
		public ListVideosOutput(
			int page,
			int perPage,
			int total,
			IReadOnlyList<VideoModelOutput> items)
			: base(page, perPage, total, items)
		{ }
	}
}
