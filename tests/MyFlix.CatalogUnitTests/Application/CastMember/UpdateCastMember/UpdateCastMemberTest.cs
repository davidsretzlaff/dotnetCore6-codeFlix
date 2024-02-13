
using Bogus;
using FluentAssertions;
using Moq;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Repository;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.CastMember.UpdateCastMember;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Application.UseCases.CastMember.UpdateCastMember;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Domain.Exceptions;
namespace MyFlix.Catalog.UnitTests.Application.CastMember.UpdateCastMember
{
	[Collection(nameof(UpdateCastMemberTestFixture))]
	public class UpdateCastMemberTest
	{
		private readonly UpdateCastMemberTestFixture _fixture;

		public UpdateCastMemberTest(UpdateCastMemberTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(Update))]
		[Trait("Application", "UpdateCastMember - Use Cases")]
		public async Task Update()
		{
			var repositoryMock = new Mock<ICastMemberRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var castMemberExample = _fixture.GetExampleCastMember();
			var newName = _fixture.GetValidName();
			var newType = _fixture.GetRandomCastMemberType();
			var input = new UpdateCastMemberInput(castMemberExample.Id, newName, newType);
			repositoryMock.Setup(x => x.Get(
				castMemberExample.Id,
				It.IsAny<CancellationToken>())
			).ReturnsAsync(castMemberExample);
			var useCase = new UseCase.UpdateCastMember(repositoryMock.Object, unitOfWorkMock.Object);

			CastMemberModelOutput output = await useCase.Handle(input, CancellationToken.None);

			output.Should().NotBeNull();
			output.Id.Should().Be(input.Id);
			output.Name.Should().Be(input.Name);
			output.Type.Should().Be(input.Type);

			repositoryMock.Verify(x => x.Get(
				  castMemberExample.Id,
				It.IsAny<CancellationToken>())
				, Times.Once);

			repositoryMock.Verify(x => x.Update(
				It.Is<DomainEntity.CastMember>(
					x => (
						x.Id == castMemberExample.Id &&
						x.Name == input.Name &&
						x.Type == input.Type
					)
				),
				It.IsAny<CancellationToken>())
			, Times.Once);
			unitOfWorkMock.Verify(x => x.Commit(
				It.IsAny<CancellationToken>())
			, Times.Once);

		}

		[Fact(DisplayName = nameof(ThrowWhenNotFound))]
		[Trait("Application", "UpdateCastMember - Use Cases")]
		public async Task ThrowWhenNotFound()
		{
			var repositoryMock = new Mock<ICastMemberRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var input = new UpdateCastMemberInput(Guid.NewGuid(), _fixture.GetValidName(), _fixture.GetRandomCastMemberType());
			repositoryMock.Setup(x => x.Get(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>())
			).ThrowsAsync(new NotFoundException("error"));
			var useCase = new UseCase.UpdateCastMember(repositoryMock.Object, unitOfWorkMock.Object);

			var action = async () => await useCase.Handle(input, CancellationToken.None);

			await action.Should().ThrowAsync<NotFoundException>();
		}

		[Fact(DisplayName = nameof(ThrowWhenNotFound))]
		[Trait("Application", "UpdateCastMember - Use Cases")]
		public async Task ThrowWhenInvalidName()
		{
			var repositoryMock = new Mock<ICastMemberRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var castMemberExample = _fixture.GetExampleCastMember();
			var input = new UpdateCastMemberInput(castMemberExample.Id, null!, _fixture.GetRandomCastMemberType());
			repositoryMock.Setup(x => x.Get(
				castMemberExample.Id,
				It.IsAny<CancellationToken>())
			).ReturnsAsync(castMemberExample);
			var useCase = new UseCase.UpdateCastMember(repositoryMock.Object, unitOfWorkMock.Object);

			var action = async () => await useCase.Handle(input, CancellationToken.None);

			await action.Should().ThrowAsync<EntityValidationException>().WithMessage("Name should not be empty or null");
		}
	}
}
