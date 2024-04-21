using MediatR;
using MyFlix.Catalog.Application.UseCases.Video.Common;

namespace MyFlix.Catalog.Application.UseCases.Video.GetVideo
{
	public record GetVideoInput(Guid VideoId) : IRequest<VideoModelOutput>;
}
