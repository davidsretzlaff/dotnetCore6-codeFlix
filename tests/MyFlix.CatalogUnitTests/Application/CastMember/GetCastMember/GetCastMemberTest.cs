using Moq;
using MyFlix.Catalog.Domain.Repository;
using UserCase = MyFlix.Catalog.Application.UseCases.CastMember.GetCastMember;
using Xunit;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using FluentAssertions;

namespace MyFlix.Catalog.UnitTests.Application.CastMember.GetCastMember
{
	[Collection(nameof(GetCastMemberTestFixture))]
	public class GetCastMemberTest
	{
		private readonly GetCastMemberTestFixture _fixture;

		public GetCastMemberTest(GetCastMemberTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(GetCastMember))]
		[Trait("Application", "GetCastMember - Use Cases")]
		public async Task GetCastMember()
		{
			var repositoryMock = new Mock<ICastMemberRepository>();
			var castMemberExample = _fixture.GetExampleCastMember();
			repositoryMock
				.Setup(x => x.Get(It.IsAny<Guid>(),It.IsAny<CancellationToken>()))
				.ReturnsAsync(castMemberExample);
			var input = new UserCase.GetCastMemberInput(castMemberExample.Id);
			var useCase = new UserCase.GetCastMember(repositoryMock.Object);

			CastMemberModelOutput output = await useCase.Handle(input, CancellationToken.None);
			
			output.Should().NotBeNull();
			output.Id.Should().Be(castMemberExample.Id);
			output.Name.Should().Be(castMemberExample.Name);
			output.Type.Should().Be(castMemberExample.Type);
			repositoryMock.Verify(x => x.Get(
				It.Is<Guid>(x => x == input.Id),
				It.IsAny<CancellationToken>()
			), Times.Once());
		}
	}
}
