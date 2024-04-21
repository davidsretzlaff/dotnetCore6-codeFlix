using MediatR;
using MyFlix.Catalog.Application.UseCases.Video.Common;

namespace MyFlix.Catalog.Application.UseCases.Video.GetVideo
{
	public interface IGetVideo : IRequestHandler<GetVideoInput, VideoModelOutput>
	{ }
}
