using Moq;
using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.ListVideos;

namespace MyFlix.Catalog.UnitTests.Application.Video.ListVideos
{
	public class ListVideosTest
	{
		private readonly ListVideosTestFixture _fixture;
		private readonly Mock<IVideoRepository> _videoRepositoryMock;
		private readonly UseCase.ListVideos _useCase;

		public ListVideosTest(ListVideosTestFixture fixture)
		{
			_fixture = fixture;
			_videoRepositoryMock = new Mock<IVideoRepository>();
			_useCase = new UseCase.ListVideos(_videoRepositoryMock.Object);
		}

		[Fact(DisplayName = nameof(ListVideos))]
		[Trait("Application", "ListVideos - Use Cases")]
		public async Task ListVideos()
		{
			var exampleVideosList = _fixture.CreateExampleVideosList();
			var input = new ListVideosInput(1, 10, "", "", SearchOrder.Asc);
			_videoRepositoryMock.Setup(x =>
				x.Search(
					It.Is<SearchInput>(x =>
						x.Page == input.Page &&
						x.PerPage == input.PerPage &&
						x.Search == input.Search &&
						x.OrderBy == input.Sort &&
						x.Order == input.Dir),
					It.IsAny<CancellationToken>()
				).ReturnsAsync(exampleVideosList)
			);

			PaginatedListOutput<DomainEntities.Video> output = await _useCase.Handle(input, CancellationToken.None);
		}
	}
}
