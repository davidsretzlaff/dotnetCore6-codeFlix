using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;
using DomainEntities = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.UnitTests.Application.Video.ListVideos
{

	[CollectionDefinition(nameof(ListVideosTestFixture))]
	public class ListVideosTestFixtureCollection
		: ICollectionFixture<ListVideosTestFixture>
	{ }

	public class ListVideosTestFixture : VideoTestFixtureBase
	{
		public List<DomainEntities.Video> CreateExampleVideosList()
			=> Enumerable.Range(1, Random.Shared.Next(2, 10))
				.Select(_ => GetValidVideoWithAllProperties())
				.ToList();
	}
}
