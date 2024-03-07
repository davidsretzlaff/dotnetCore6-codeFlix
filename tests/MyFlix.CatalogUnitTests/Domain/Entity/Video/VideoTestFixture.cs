
using MyFlix.Catalog.UnitTests.Common;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.Video
{
	[CollectionDefinition(nameof(VideoTestFixture))]
	public class VideoTestFixtureCollection : ICollectionFixture<VideoTestFixture>
	{ }

	public class VideoTestFixture : BaseFixture
	{
		public DomainEntity.Video GetValidVideo() => new DomainEntity.Video("Title","Description",2001,true,true,180);
	}
}
