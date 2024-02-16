using MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.Common;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.CreateMember
{

	[CollectionDefinition(nameof(CreateCastMemberTestFixture))]
	public class CreateCastMemberTestFixtureCollection : ICollectionFixture<CreateCastMemberTestFixture>
	{
	}

	public class CreateCastMemberTestFixture : CastMemberUseCasesBaseFixture
	{
	}
}
