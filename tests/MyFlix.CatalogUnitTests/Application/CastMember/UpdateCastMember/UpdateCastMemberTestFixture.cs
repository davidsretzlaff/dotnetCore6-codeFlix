using MyFlix.Catalog.UnitTests.Application.CastMember.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.CastMember.UpdateCastMember
{
	[CollectionDefinition(nameof(UpdateCastMemberTestFixture))]
	public class UpdateCastMemberTestFixtureCollection : ICollectionFixture<UpdateCastMemberTestFixture>
	public class UpdateCastMemberTestFixture : CastMemberUseCasesBaseFixture
	{
	}
}