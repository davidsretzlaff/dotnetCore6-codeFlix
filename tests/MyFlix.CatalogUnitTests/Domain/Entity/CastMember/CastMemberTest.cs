﻿using Xunit;
using Entity = MyFlix.Catalog.Domain.Entity;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using FluentAssertions;
using MyFlix.Catalog.Domain.Exceptions;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.CastMember
{

	[Collection(nameof(CastMemberTestFixture))]
	public class CastMemberTest
	{
		private CastMemberTestFixture _fixture;

		public CastMemberTest(CastMemberTestFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Instantiate))]
		[Trait("Domain", "CastMember - Aggregates")]
		public void Instantiate()
		{
			var datetimeBefore = DateTime.Now.AddSeconds(-1);
			var name = _fixture.GetValidName();
			var type = _fixture.GetRandomCastMemberType();

			var castMember = new DomainEntity.CastMember(name, type);

			var datetimeAfter = DateTime.Now.AddSeconds(1);
			castMember.Id.Should().NotBe(default(Guid));
			castMember.Name.Should().Be(name);
			castMember.Type.Should().Be(type);
			(castMember.CreatedAt >= datetimeBefore).Should().BeTrue();
			(castMember.CreatedAt <= datetimeAfter).Should().BeTrue();
		}

		[Theory(DisplayName = nameof(ThrowErrorWhenNameIsInvalid))]
		[Trait("Domain", "CastMember - Aggregates")]
		[InlineData("")]
		[InlineData("   ")]
		[InlineData(null)]
		public void ThrowErrorWhenNameIsInvalid(string? name)
		{
			var datetimeBefore = DateTime.Now.AddSeconds(-1);
			var type = _fixture.GetRandomCastMemberType();

			var action = () => new DomainEntity.CastMember(name!, type);

			action.Should().Throw<EntityValidationException>().WithMessage($"Name should not be empty or null");
		}
	}
}
