
using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Video.ListVideos
{
	[CollectionDefinition(nameof(ListVideosTestFixture))]
	public class ListVideosTestFixtureCollection
		: ICollectionFixture<ListVideosTestFixture>
	{ }

	public class ListVideosTestFixture : VideoTestFixtureBase
	{
	}
}
