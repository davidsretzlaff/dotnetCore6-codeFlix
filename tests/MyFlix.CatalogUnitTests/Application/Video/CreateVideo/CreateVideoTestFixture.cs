using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Video.CreateVideo
{
	[CollectionDefinition(nameof(CreateVideoTestFixture))]
	public class CreateVideoTestFixtureCollection
		: ICollectionFixture<CreateVideoTestFixture>
	{ }

	public class CreateVideoTestFixture : VideoTestFixtureBase
	{ }
}
