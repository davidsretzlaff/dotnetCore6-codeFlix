using MyFlix.Catalog.UnitTests.Application.CastMember.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.CastMember.DeleteCastMember
{
	[CollectionDefinition(nameof(DeleteCastMemberTestFixture))]
	public class DeleteCastMemberFixtureCollection : ICollectionFixture<DeleteCastMemberTestFixture> { }
	public class DeleteCastMemberTestFixture : CastMemberUseCasesBaseFixture
	{
	}
}
