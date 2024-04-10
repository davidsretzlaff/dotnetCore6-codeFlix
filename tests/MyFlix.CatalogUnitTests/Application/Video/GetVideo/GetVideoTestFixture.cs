using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Video.GetVideo
{

	[CollectionDefinition(nameof(GetVideoTestFixture))]
	public class GetVideoTestFixtureCollection
		: ICollectionFixture<GetVideoTestFixture>
	{ }

	public class GetVideoTestFixture : VideoTestFixtureBase
	{ }
}
