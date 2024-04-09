using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Video.DeleteVideo
{
	public record DeleteVideoInput(Guid VideoId) : IRequest;
}
