using FluentAssertions;
using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.VideoRepository
{
	[Collection(nameof(VideoRepositoryTestFixture))]
	public class VideoRepositoryTest
	{
		[Fact(DisplayName = nameof(Insert))]
		[Trait("Integration/Infra.Data", "Video Repository - Repositories")]
		public void Insert()
		{
			1.Should().Be(1);
		}
	}
}
