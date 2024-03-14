using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.Video
{
	[CollectionDefinition(nameof(VideoTestFixture))]
	public class VideoTestFixtureCollection : ICollectionFixture<VideoTestFixture>
	{ }
	public class VideoTestFixture : VideoTestFixtureBase
	{ }
}
