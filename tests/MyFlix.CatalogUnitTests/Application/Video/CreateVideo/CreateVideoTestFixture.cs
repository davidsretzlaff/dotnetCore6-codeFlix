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
		internal CreateVideoInput CreateValidCreateVideoInput( List<Guid>? categoriesIds = null, List<Guid>? genresIds = null) 
			=> new (
				GetValidTitle(),
				GetValidDescription(),
				GetValidYearLaunched(),
				GetRandomBoolean(),
				GetRandomBoolean(),
				GetValidDuration(),
				GetRandomRating(),
				categoriesIds,
				Genre
			);
	}
}
