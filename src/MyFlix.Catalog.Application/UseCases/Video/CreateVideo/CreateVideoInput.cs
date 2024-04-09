using MediatR;
using MyFlix.Catalog.Application.UseCases.Video.Common;
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
		Rating Rating,
		IReadOnlyCollection<Guid>? CategoriesIds = null,
		IReadOnlyCollection<Guid>? GenresIds = null,
		IReadOnlyCollection<Guid>? CastMembersIds = null,
		FileInput? Thumb = null,
		FileInput? Banner = null,
		FileInput? ThumbHalf = null,
		FileInput? Media = null,
		FileInput? Trailer = null
	) : IRequest<CreateVideoOutput>;
}
