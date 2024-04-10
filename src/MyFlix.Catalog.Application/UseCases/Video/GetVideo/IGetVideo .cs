using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Video.GetVideo
{
	public interface IGetVideo : IRequestHandler<GetVideoInput, GetVideoOutput>
	{ }
}
