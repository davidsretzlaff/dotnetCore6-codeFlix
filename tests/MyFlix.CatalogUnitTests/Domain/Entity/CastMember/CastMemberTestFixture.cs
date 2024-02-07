
using MyFlix.Catalog.UnitTests.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.CastMember
{
	[CollectionDefinition(nameof(CastMemberTestFixture))]
	public class CastMemberTestFixtureCollection : ICollectionFixture<CastMemberTestFixture>
	{ }

	public class CastMemberTestFixture : BaseFixture
	{
		public string GetValidName() 
			=> Faker.Name.FullName();

		public CastMemberType GetRandomCastMemberType()
			=> (new Random()).Next(1, 2) as CastMemberType;
	}
}
