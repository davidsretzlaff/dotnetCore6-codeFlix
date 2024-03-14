using MediatR;
using MyFlix.Catalog.Domain.Enum;

namespace MyFlix.Catalog.Application.UseCases.Video.CreateVideo
{
	public record CreateVideoInput(
		string Title,
		string Description,
		int YearLaunched,
		bool Opened,
		bool Published,
		int Duration,
		Rating Rating
	) : IRequest<CreateVideoOutput>;
}
