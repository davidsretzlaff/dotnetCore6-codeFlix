
using MyFlix.Catalog.Domain.Enum;
using MyFlix.Catalog.UnitTests.Common;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

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
			=> (CastMemberType)(new Random()).Next(1, 2);

		public DomainEntity.CastMember GetExampleCastMember()
		   => new DomainEntity.CastMember(GetValidName(), GetRandomCastMemberType());

	}
}
