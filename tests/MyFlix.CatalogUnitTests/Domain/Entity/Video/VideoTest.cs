
using Xunit;
using FluentAssertions;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Exceptions;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.Video
{
	[Collection(nameof(VideoTestFixture))]
	public class VideoTest
	{
		private readonly VideoTestFixture _fixture;

		public VideoTest(VideoTestFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Instantiate))]
		[Trait("Domain", "Video - Aggregate")]
		public void Instantiate()
		{
			var expectedTitle = _fixture.GetValidTitle();
			var expectedDescription = _fixture.GetValidDescription();
			var expectedYearLaunched = _fixture.GetValidYearLaunched();
			var expectedOpened = _fixture.GetRandomBoolean();
			var expectedPublished = _fixture.GetRandomBoolean();
			var expectedDuration = _fixture.GetValidDuration();

			var expectedCreatedDate = DateTime.Now;
			var video = new DomainEntity.Video(
				expectedTitle,
				expectedDescription,
				expectedYearLaunched,
				expectedOpened,
				expectedPublished,
				expectedDuration
			);

			video.Title.Should().Be("Title");
			video.Description.Should().Be("Description");
			video.Opened.Should().Be(true);
			video.Published.Should().Be(true);
			video.YearLaunched.Should().Be(2001);
			video.Duration.Should().Be(180);
			video.CreatedAt.Should().BeCloseTo(expectedCreatedDate, TimeSpan.FromSeconds(10));
		}

		[Fact(DisplayName = nameof(InstantiateThrowsExceptionWhrnNotValid))]
		[Trait("Domain", "Video - Aggregate")]
		public void InstantiateThrowsExceptionWhrnNotValid()
		{
			var expectedTitle = "";
			var expectedDescription = _fixture.GetTooLongDescription();
			var expectedYearLaunched = _fixture.GetValidYearLaunched();
			var expectedOpened = _fixture.GetRandomBoolean();
			var expectedPublished = _fixture.GetRandomBoolean();
			var expectedDuration = _fixture.GetValidDuration();

			var expectedCreatedDate = DateTime.Now;
			var action = () => new DomainEntity.Video(
				expectedTitle,
				expectedDescription,
				expectedYearLaunched,
				expectedOpened,
				expectedPublished,
				expectedDuration
			);

			action.Should().Throw<EntityValidationException>().WithMessage("Validation errors");
		}
	}
}
