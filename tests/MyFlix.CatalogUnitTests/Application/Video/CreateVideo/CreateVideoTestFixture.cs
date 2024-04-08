using MyFlix.Catalog.Application.UseCases.Video.Common;
using MyFlix.Catalog.Application.UseCases.Video.CreateVideo;
using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Video.CreateVideo
{
	[CollectionDefinition(nameof(CreateVideoTestFixture))]
	public class CreateVideoTestFixtureCollection
		: ICollectionFixture<CreateVideoTestFixture>
	{ }

	public class CreateVideoTestFixture : VideoTestFixtureBase
	{
		internal CreateVideoInput CreateValidInput(
			List<Guid>? categoriesIds = null, 
			List<Guid>? genresIds = null, 
			List<Guid>? castMembersIds = null, 
			FileInput? thumb = null,
			FileInput? banner = null,
			FileInput? thumbHalf = null
		) 
			=> new (
				GetValidTitle(),
				GetValidDescription(),
				GetValidYearLaunched(),
				GetRandomBoolean(),
				GetRandomBoolean(),
				GetValidDuration(),
				GetRandomRating(),
				categoriesIds,
				genresIds,
				castMembersIds,
				thumb,
				banner,
				thumbHalf
		);

		internal CreateVideoInput CreateValidInputWithAllImages() => new(
			 GetValidTitle(),
			 GetValidDescription(),
			 GetValidYearLaunched(),
			 GetRandomBoolean(),
			 GetRandomBoolean(),
			 GetValidDuration(),
			 GetRandomRating(),
			 null,
			 null,
			 null,
			 GetValidImageFileInput(),
			 GetValidImageFileInput(),
			 GetValidImageFileInput()
		 );
	}
}
