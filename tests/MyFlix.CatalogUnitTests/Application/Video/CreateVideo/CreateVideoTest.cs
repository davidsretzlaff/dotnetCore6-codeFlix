using Moq;
using MyFlix.Catalog.Application.Interfaces;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.Video.CreateVideo;
using DomainEntities = MyFlix.Catalog.Domain.Entity;
using FluentAssertions;
using MyFlix.Catalog.Domain.Repository;

namespace MyFlix.Catalog.UnitTests.Application.Video.CreateVideo
{
	[Collection(nameof(CreateVideoTestFixture))]
	public class CreateVideoTest
	{
		private readonly CreateVideoTestFixture _fixture;

		public CreateVideoTest(CreateVideoTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(CreateVideo))]
		public async Task CreateVideo()
		{
			var repositoryMock = new Mock<IVideoRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var useCase = new UseCase.CreateVideo(
				repositoryMock.Object,
				unitOfWorkMock.Object
			);
			var input = new UseCase.CreateVideoInput(
				_fixture.GetValidTitle(),
				_fixture.GetValidDescription(),
				_fixture.GetValidYearLaunched(),
				_fixture.GetRandomBoolean(),
				_fixture.GetRandomBoolean(),
				_fixture.GetValidDuration(),
				_fixture.GetRandomRating()
			);

			var output = await useCase.Handle(input, CancellationToken.None);

			repositoryMock.Verify(x => x.Insert(
				It.Is<DomainEntities.Video>(
					video =>
						video.Title == input.Title &&
						video.Published == input.Published &&
						video.Description == input.Description &&
						video.Duration == input.Duration &&
						video.Rating == input.Rating &&
						video.Id != Guid.Empty &&
						video.YearLaunched == input.YearLaunched &&
						video.Opened == input.Opened
				),
				It.IsAny<CancellationToken>())
			);
			unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
			output.Id.Should().NotBeEmpty();
			output.CreatedAt.Should().NotBe(default(DateTime));
			output.Title.Should().Be(input.Title);
			output.Published.Should().Be(input.Published);
			output.Description.Should().Be(input.Description);
			output.Duration.Should().Be(input.Duration);
			output.Rating.Should().Be(input.Rating);
			output.YearLaunched.Should().Be(input.YearLaunched);
			output.Opened.Should().Be(input.Opened);
		}
	}
}
