using MyFlix.Catalog.Domain.Enum;
using MyFlix.Catalog.UnitTests.Common;
using DomainEntity = MyFlix.Catalog.Domain.Entity.CastMember;

namespace MyFlix.Catalog.UnitTests.Application.CastMember.Common
{
	public class CastMemberUseCasesBaseFixture : BaseFixture
	{
		public string GetValidName()
		=> Faker.Name.FullName();

		public CastMemberType GetRandomCastMemberType()
			=> (CastMemberType)(new Random()).Next(1, 2);

	}
}
