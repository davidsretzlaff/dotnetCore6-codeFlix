
using Xunit;

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

		}
	}
}
