﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Repository = MyFlix.Catalog.Infra.Data.EF.Repositories;
namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CastMemberRepository
{
	[Collection(nameof(CastMemberRepositoryTestFixture))]
	public class CastMemberRepositoryTest
	{
		private readonly CastMemberRepositoryTestFixture _fixture;

		public CastMemberRepositoryTest(CastMemberRepositoryTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName =nameof(Insert))]
		[Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
		public async Task Insert()
		{
			var castMemberExample = _fixture.GetExampleCastMember();
			var context = _fixture.CreateDbContext();
			var repository = new Repository.CastMemberRepository(context);

			await repository.Insert(castMemberExample, CancellationToken.None);
			context .SaveChanges();

			// persist state
			var assertionContext = _fixture.CreateDbContext(true);
			var castMemberFromDb = await assertionContext.CastMembers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == castMemberExample.Id);
			castMemberFromDb.Should().NotBeNull();
			castMemberFromDb!.Name.Should().Be(castMemberExample.Name);
			castMemberFromDb.Type.Should().Be(castMemberExample.Type);
		}

		[Fact(DisplayName = nameof(Get))]
		[Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
		public async Task Get()
		{
			var castMemberExampleList = _fixture.GetExampleCastMemberList(5);
			var castMemberExample = castMemberExampleList[3];
			var arrangeContext = _fixture.CreateDbContext();
			await arrangeContext.AddRangeAsync(castMemberExampleList);
			await arrangeContext.SaveChangesAsync();
			var repository = new Repository.CastMemberRepository(_fixture.CreateDbContext(true));

			var itemFromRepository = await repository.Get(castMemberExample.Id, CancellationToken.None);

			itemFromRepository.Should().NotBeNull();
			itemFromRepository!.Name.Should().Be(castMemberExample.Name);
			itemFromRepository.Type.Should().Be(castMemberExample.Type);
		}
	}
}
