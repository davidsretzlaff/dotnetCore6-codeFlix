using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Video.ListVideo
{
	public interface IListVideos : IRequestHandler<ListVideosInput, ListVideosOutput>
	{ }
}
