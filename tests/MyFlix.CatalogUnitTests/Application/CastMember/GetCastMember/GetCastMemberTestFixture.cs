using MyFlix.Catalog.UnitTests.Application.CastMember.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.CastMember.GetCastMember
{
	[CollectionDefinition(nameof(GetCastMemberTestFixture))]
	public class GetCastMemberTestFixtureCollection : ICollectionFixture<GetCastMemberTestFixture> { }
	public class GetCastMemberTestFixture : CastMemberUseCasesBaseFixture
	{
	}
}
