using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Video.GetVideo
{
	public record GetVideoInput(Guid VideoId) : IRequest<GetVideoOutput>;
}
