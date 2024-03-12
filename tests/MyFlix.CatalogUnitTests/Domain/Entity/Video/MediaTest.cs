
using FluentAssertions;
using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Enum;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.Video
{

	[Collection(nameof(VideoTestFixture))]
	public class MediaTest
	{
		private readonly VideoTestFixture _fixture;

		public MediaTest(VideoTestFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Instantiate))]
		[Trait("Domain", "Media - Entities")]
		public void Instantiate()
		{
			var expectedFilePath = _fixture.GetValidMediaPath();

			var media = new Media(expectedFilePath);

			media.FilePath.Should().Be(expectedFilePath);
			media.Status.Should().Be(MediaStatus.Pending);
		}

		[Fact(DisplayName = nameof(UpdateAsSentToEncode))]
		[Trait("Domain", "Media - Entities")]
		public void UpdateAsSentToEncode()
		{
			var media = _fixture.GetValidMedia();

			media.UpdateAsSentToEncode();

			media.Status.Should().Be(MediaStatus.Processing);
		}
	}
}
