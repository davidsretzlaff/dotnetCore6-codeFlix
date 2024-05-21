using MyFlix.Catalog.IntegrationTest.Base;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.VideoRepository
{
	[CollectionDefinition(nameof(VideoRepositoryTestFixture))]
	public class VideoRepositoryTestFixtureCollection : ICollectionFixture<VideoRepositoryTestFixture>
	{
	}

	public class VideoRepositoryTestFixture : BaseFixture
	{
	}
}
