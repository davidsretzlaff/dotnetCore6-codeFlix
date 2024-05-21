using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.Video.UpdateVideo;

namespace MyFlix.Catalog.UnitTests.Application.Video.UploadVideo
{
	[CollectionDefinition(nameof(UpdateVideoTestFixture))]
	public class UpdateVideoTestFixtureCollection : ICollectionFixture<UpdateVideoTestFixture>
	{ }

	public class UpdateVideoTestFixture : VideoTestFixtureBase
	{
		public UseCase.UpdateVideoInput CreateValidInput(Guid videoId)
		  => new(
			  videoId,
			  GetValidTitle(),
			  GetValidDescription(),
			  GetValidYearLaunched(),
			  GetRandomBoolean(),
			  GetRandomBoolean(),
			  GetValidDuration(),
			  GetRandomRating()
		  );
	}
}
