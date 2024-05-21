using MediatR;
using MyFlix.Catalog.Application.UseCases.Video.Common;

namespace MyFlix.Catalog.Application.UseCases.Video.UpdateVideo
{
	public interface IUpdateVideo : IRequestHandler<UpdateVideoInput, VideoModelOutput>
	{ }
}
