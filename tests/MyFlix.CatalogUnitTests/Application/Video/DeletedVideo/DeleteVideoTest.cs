using Moq;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Domain.Repository;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.Video.DeleteVideo;

namespace MyFlix.Catalog.UnitTests.Application.Video.DeletedVideo
{
	[Collection(nameof(DeleteVideoTestFixture))]
	public class DeleteVideoTest
	{
		private readonly DeleteVideoTestFixture _fixture;
		private readonly UseCase.DeleteVideo _useCase;
		private readonly Mock<IVideoRepository> _repositoryMock;
		private readonly Mock<IUnitOfWork> _unitOfWorkMock;

		public DeleteVideoTest(DeleteVideoTestFixture fixture)
		{
			_fixture = fixture;
			_repositoryMock = new Mock<IVideoRepository>();
			_unitOfWorkMock = new Mock<IUnitOfWork>();
			_useCase = new UseCase.DeleteVideo(
				_repositoryMock.Object,
				_unitOfWorkMock.Object
			);
		}

		[Fact(DisplayName = nameof(DeleteVideo))]
		[Trait("Application", "DeleteVideo - Use Cases")]
		public void DeleteVideo()
		{

		}
	}
