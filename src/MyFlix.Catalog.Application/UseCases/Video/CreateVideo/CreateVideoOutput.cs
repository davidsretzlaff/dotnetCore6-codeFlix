using MyFlix.Catalog.Domain.Enum;

namespace MyFlix.Catalog.Application.UseCases.Video.CreateVideo
{
	public record CreateVideoOutput(
		Guid Id,
		DateTime CreatedAt,
		string Title,
		bool Published,
		string Description,
		Rating Rating,
		int YearLaunched,
		bool Opened
	);
}
