using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Video.DeleteVideo
{
	public interface IDeleteVideo : IRequestHandler<DeleteVideoInput>
	{ }
}
