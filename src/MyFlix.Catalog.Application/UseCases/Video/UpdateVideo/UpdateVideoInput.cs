using MediatR;
using MyFlix.Catalog.Application.UseCases.Video.Common;
using MyFlix.Catalog.Domain.Enum;

namespace MyFlix.Catalog.Application.UseCases.Video.UpdateVideo
{
	public record UpdateVideoInput(
		Guid VideoId,
		string Title,
		string Description,
		int YearLaunched,
		bool Opened,
		bool Published,
		int Duration,
		Rating Rating
	) : IRequest<VideoModelOutput>;
}
