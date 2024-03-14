using MediatR;

namespace MyFlix.Catalog.Application.UseCases.Video.CreateVideo
{
	public interface ICreateVideo : IRequestHandler<CreateVideoInput, CreateVideoOutput>
	{ }
}
