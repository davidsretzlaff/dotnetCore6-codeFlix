using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.UnitTests.Application.CastMember.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.CastMember.ListCastMember
{
	[CollectionDefinition(nameof(ListCastMemberTestFixture))]
	public class ListCastMemberTestCollection : ICollectionFixture<ListCastMemberTestFixture> { }
	public class ListCastMemberTestFixture : CastMemberUseCasesBaseFixture
	{
		public List<DomainEntity.CastMember> GetExampleCastMemberList(int quantity)
		{
			return Enumerable.Range(1, quantity).Select(_ => GetExampleCastMember()).ToList();
		}
	}
}
