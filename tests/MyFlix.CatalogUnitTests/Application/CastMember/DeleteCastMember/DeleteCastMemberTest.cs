using Moq;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Domain.Repository;
using UseCase = MyFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
using Xunit;
using FluentAssertions;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.UnitTests.Application.CastMember.DeleteCastMember
{
	[Collection(nameof(DeleteCastMemberTestFixture))]
	public class DeleteCastMemberTest
	{
		private readonly DeleteCastMemberTestFixture _fixture;

		public DeleteCastMemberTest(DeleteCastMemberTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(DeleteCastMember))]
		[Trait("Application","DeleteCastMember - Use Cases")]
		public async Task DeleteCastMember()
		{
			var repositoryMock = new Mock<ICastMemberRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var castMemberExample = _fixture.GetExampleCastMember();
			repositoryMock
				.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(castMemberExample);
			var input = new UseCase.DeleteCastMemberInput(castMemberExample.Id);
			var useCase = new UseCase.DeleteCastMember(repositoryMock.Object, unitOfWorkMock.Object);

			var action = async () => await useCase.Handle(input, CancellationToken.None);

			await action.Should().NotThrowAsync();
			repositoryMock.Verify(
				x => x.Get(It.Is<Guid>(x => x == input.Id), It.IsAny<CancellationToken>()),
				Times.Once
			);

			repositoryMock.Verify(
				x => x.Delete(It.Is<DomainEntity.CastMember>(x => x.Id == input.Id), 
				It.IsAny<CancellationToken>()),
				Times.Once
			);

			unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), 
				Times.Once
			);
		}
	}
}
