using MyFlix.Catalog.Domain.Enum;
using DomainEntities = MyFlix.Catalog.Domain.Entity;

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
		bool Opened,
		int Duration,
		IReadOnlyCollection<Guid> CategoriesIds,
		IReadOnlyCollection<Guid> GenresIds
	)
	{
		public static CreateVideoOutput FromVideo(DomainEntities.Video video) => new(
			video.Id,
			video.CreatedAt,
			video.Title,
			video.Published,
			video.Description,
			video.Rating,
			video.YearLaunched,
			video.Opened,
			video.Duration,
			video.Categories,
			video.Genres
		);
	}
}
