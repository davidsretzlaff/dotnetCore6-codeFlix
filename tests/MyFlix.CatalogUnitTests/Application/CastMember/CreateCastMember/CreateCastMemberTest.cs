﻿using Moq;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Domain.Enum;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.CastMember.CreateCastMember;
using MyFlix.Catalog.Domain.Repository;
using FluentAssertions;

namespace MyFlix.Catalog.UnitTests.Application.CastMember.CreateCastMember
{

	[Collection(nameof(CreateCastMemberTestFixture))]
	public class CreateCastMemberTest
	{
		private readonly CreateCastMemberTestFixture _fixture;

		public CreateCastMemberTest(CreateCastMemberTestFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Create))]
		[Trait("Application", "CreateCastMember - Use Cases")]
		public async Task Create()
		{
			var input = new UseCase.CreateCastMemberInput(
				_fixture.GetValidName(), 
				_fixture.GetRandomCastMemberType()
			);
			var repositoryMock = new Mock<ICastMemberRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var useCase = new UseCase.CreateCastMember(repositoryMock.Object, unitOfWorkMock.Object);

			var output = await useCase.Handle(input, CancellationToken.None);

			output.Should().NotBeNull();
			output.Id.Should().NotBeEmpty();
			output.Name.Should().Be(input.Name);
			output.Type.Should().Be(input.Type);
			output.CreatedAt.Should().NotBeSameDateAs(default);
			unitOfWorkMock.Verify(
				x => x.Commit(It.IsAny<CancellationToken>()),
				Times.Once
			);
			repositoryMock.Verify(
				x => x.Insert(
					It.Is<DomainEntity.CastMember>(
						x => (x.Name == input.Name && x.Type == input.Type)
					),
					It.IsAny<CancellationToken>()
				), Times.Once
			);
		}
	}
}