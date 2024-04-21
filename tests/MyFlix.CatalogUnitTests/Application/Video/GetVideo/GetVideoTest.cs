using Moq;
using MyFlix.Catalog.Domain.Repository;
using UseCase = MyFlix.Catalog.Application.UseCases.Video.GetVideo;
using Xunit;
using FluentAssertions;
using MyFlix.Catalog.Application.Exceptions;

namespace MyFlix.Catalog.UnitTests.Application.Video.GetVideo
{
	[Collection(nameof(GetVideoTestFixture))]
	public class GetVideoTest
	{
		private readonly GetVideoTestFixture _fixture;

		public GetVideoTest(GetVideoTestFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Get))]
		[Trait("Application", "GetVideo - Use Cases")]
		public async Task Get()
		{
			var exampleVideo = _fixture.GetValidVideo();
			var repositoryMock = new Mock<IVideoRepository>();
			repositoryMock.Setup(x => x.Get(
				It.Is<Guid>(id => id == exampleVideo.Id),
				It.IsAny<CancellationToken>())
			).ReturnsAsync(exampleVideo);
			var useCase = new UseCase.GetVideo(repositoryMock.Object);
			var input = new UseCase.GetVideoInput(exampleVideo.Id);

			var output = await useCase.Handle(input, CancellationToken.None);

			output.Should().NotBeNull();
			output.Id.Should().Be(exampleVideo.Id);
			output.CreatedAt.Should().Be(exampleVideo.CreatedAt);
			output.Title.Should().Be(exampleVideo.Title);
			output.Published.Should().Be(exampleVideo.Published);
			output.Description.Should().Be(exampleVideo.Description);
			output.Duration.Should().Be(exampleVideo.Duration);
			output.Rating.Should().Be(exampleVideo.Rating);
			output.YearLaunched.Should().Be(exampleVideo.YearLaunched);
			output.Opened.Should().Be(exampleVideo.Opened);
			repositoryMock.VerifyAll();
		}

		[Fact(DisplayName = nameof(ThrowsExceptionWhenNotFound))]
		[Trait("Application", "GetVideo - Use Cases")]
		public async Task ThrowsExceptionWhenNotFound()
		{
			var repositoryMock = new Mock<IVideoRepository>();
			repositoryMock.Setup(x => x.Get(
				It.IsAny<Guid>(),
				It.IsAny<CancellationToken>())
			).ThrowsAsync(new NotFoundException("Video not found"));
			var useCase = new UseCase.GetVideo(repositoryMock.Object);
			var input = new UseCase.GetVideoInput(Guid.NewGuid());

			var action = () => useCase.Handle(input, CancellationToken.None);

			await action.Should().ThrowAsync<NotFoundException>().WithMessage("Video not found");
			repositoryMock.VerifyAll();
		}
	}
}
