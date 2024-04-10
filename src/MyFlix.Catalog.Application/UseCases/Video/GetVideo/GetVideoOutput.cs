using MyFlix.Catalog.Domain.Enum;
using DomainEntities = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.Application.UseCases.Video.GetVideo
{

	public record GetVideoOutput(
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
		IReadOnlyCollection<Guid> GenresIds,
		IReadOnlyCollection<Guid> CastMembersIds,
		string? Thumb,
		string? Banner,
		string? ThumbHalf,
		string? Media,
		string? Trailer)
	{
		public static GetVideoOutput FromVideo(DomainEntities.Video video) => new(
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
			video.Genres,
			video.CastMembers,
			video.Thumb?.Path,
			video.Banner?.Path,
			video.ThumbHalf?.Path,
			video.Media?.FilePath,
			video.Trailer?.FilePath);
	}
}
