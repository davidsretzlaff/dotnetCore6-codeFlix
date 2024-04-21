using MediatR;
using MyFlix.Catalog.Application.UseCases.Video.Common;

namespace MyFlix.Catalog.Application.UseCases.Video.CreateVideo
{
	public interface ICreateVideo : IRequestHandler<CreateVideoInput, VideoModelOutput>
	{ }
}
