using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Video.DeletedVideo
{
	[CollectionDefinition(nameof(DeleteVideoTestFixture))]
	public class DeleteVideoTestFixtureCollection
	: ICollectionFixture<DeleteVideoTestFixture>
	{ }

	public class DeleteVideoTestFixture : VideoTestFixtureBase
	{
		internal DeleteVideoInput GetValidInput(Guid? id = null)
		=> new(id ?? Guid.NewGuid());
	}
}
