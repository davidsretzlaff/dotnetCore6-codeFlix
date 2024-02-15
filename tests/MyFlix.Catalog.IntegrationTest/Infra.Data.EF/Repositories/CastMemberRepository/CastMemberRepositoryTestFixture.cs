using MyFlix.Catalog.Domain.Enum;
using MyFlix.Catalog.IntegrationTest.Base;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CastMemberRepository
{
	[CollectionDefinition(nameof(CastMemberRepositoryTestFixture))]
	public class CastMemberRepositoryTestFixtureCollection : ICollectionFixture<CastMemberRepositoryTestFixture> { }
	public class CastMemberRepositoryTestFixture : BaseFixture
	{
		public List<DomainEntity.CastMember> GetExampleCastMemberList(int quantity)
		{
			return Enumerable.Range(1, quantity).Select(_ => GetExampleCastMember()).ToList();
		}
		public DomainEntity.CastMember GetExampleCastMember()
			=> new DomainEntity.CastMember(GetValidName(), GetRandomCastMemberType());

		public string GetValidName()
		=> Faker.Name.FullName();

		public CastMemberType GetRandomCastMemberType()
			=> (CastMemberType)(new Random()).Next(1, 2);

		public List<DomainEntity.CastMember> GetExampleCastMembersListByNames(List<string> names)
		{ 
			return  names.Select(name =>
				{
					var example = GetExampleCastMember();
					example.Update(name, example.Type);
					return example;
				}).ToList();
		}
	}
}
