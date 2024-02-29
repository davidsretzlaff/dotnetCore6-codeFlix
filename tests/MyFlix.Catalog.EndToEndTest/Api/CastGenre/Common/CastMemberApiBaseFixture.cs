using MyFlix.Catalog.Domain.Enum;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using MyFlix.Catalog.EndToEndTest.Base;
using System.Collections.Generic;
using System.Linq;
using System;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.CastGenre.Common
{
	[CollectionDefinition(nameof(CastMemberApiBaseFixture))]
	public class CastMemberApiBaseFixtureCollection : ICollectionFixture<CastMemberApiBaseFixture>
	{
	}

	public class CastMemberApiBaseFixture : BaseFixture
	{
		public CastMemberPersistence Persistence;

		public CastMemberApiBaseFixture()
		{
			Persistence = new CastMemberPersistence(CreateDbContext());
		}

		public DomainEntity.CastMember GetExampleCastMember()
			=> new(GetValidName(), GetRandomCastMemberType());

		public string GetValidName()
			=> Faker.Name.FullName();

		public CastMemberType GetRandomCastMemberType()
			=> (CastMemberType)(new Random()).Next(1, 2);

		public List<DomainEntity.CastMember> GetExampleCastMembersList(int quantity)
			=> Enumerable.Range(1, quantity).Select(_ => GetExampleCastMember()).ToList();

		public List<DomainEntity.CastMember> GetExampleCastMembersListByNames(List<string> names)
			=> names
				.Select(name =>
				{
					var example = GetExampleCastMember();
					example.Update(name, example.Type);
					return example;
				})
				.ToList();

		public List<DomainEntity.CastMember> CloneListOrdered(List<DomainEntity.CastMember> list,  string orderBy, SearchOrder order)
		{
			var listClone = new List<DomainEntity.CastMember>(list);
			var orderedEnumerable = (orderBy.ToLower(), order) switch
			{
				("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
				("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
				("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
				("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
				("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
				("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
				_ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
			};
			return orderedEnumerable.ToList();
		}
	}
}
