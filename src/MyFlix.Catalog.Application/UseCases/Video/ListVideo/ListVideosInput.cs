
using MediatR;
using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Application.UseCases.Video.ListVideo
{
	public class ListVideosInput : PaginatedListInput, IRequest<ListVideosOutput>
	{
		public ListVideosInput(
			int page,
			int perPage,
			string search,
			string sort,
			SearchOrder dir)
			: base(page, perPage, search, sort, dir)
		{ }
	}
}
