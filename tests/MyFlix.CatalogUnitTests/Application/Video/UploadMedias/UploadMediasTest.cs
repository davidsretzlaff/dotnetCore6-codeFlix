﻿using FluentAssertions;
using Moq;
using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Domain.Repository;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.Video.UploadMedias;

namespace MyFlix.Catalog.UnitTests.Application.Video.UploadMedias
{
    [Collection(nameof(UploadMediasTestFixture))]
    public class UploadMediasTest
    {
        private readonly UploadMediasTestFixture _fixture;
        private readonly UseCase.UploadMedias _useCase;
        private readonly Mock<IVideoRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IStorageService> _storageServiceMock;

        public UploadMediasTest(UploadMediasTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new Mock<IVideoRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _storageServiceMock = new Mock<IStorageService>();
            _useCase = new UseCase.UploadMedias(
                _repositoryMock.Object,
                _storageServiceMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact(DisplayName = nameof(UploadMedias))]
        [Trait("Application", "UploadMedias - Use Cases")]
        public async Task UploadMedias()
        {
			var video = _fixture.GetValidVideo();
			var validInput = _fixture.GetValidInput(videoId: video.Id);
			var fileNames = new List<string>() {
			    StorageFileName.Create(video.Id, nameof(video.Media), validInput.VideoFile!.Extension),
			    StorageFileName.Create(video.Id, nameof(video.Trailer), validInput.TrailerFile!.Extension)
		     };
			_repositoryMock.Setup(x => x.Get(
                It.Is<Guid>(x => x == video.Id),
				It.IsAny<CancellationToken>())
            ).ReturnsAsync(video);
            _storageServiceMock
                .Setup(x => x.Upload(
                    It.IsAny<string>(),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>())
                ).ReturnsAsync(Guid.NewGuid().ToString());

			await _useCase.Handle(validInput, CancellationToken.None);

			_repositoryMock.VerifyAll();
            _storageServiceMock.Verify(x =>
			   x.Upload(
				It.Is<string>(x => fileNames.Contains(x)),
				It.IsAny<Stream>(),
				It.IsAny<CancellationToken>()),
				Times.Exactly(2)
            );
            _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
        }

		[Fact(DisplayName = nameof(ThrowsWhenVideoNotFound))]
		[Trait("Application", "UploadMedias - Use Cases")]
		public async Task ThrowsWhenVideoNotFound()
		{
			var video = _fixture.GetValidVideo();
			var validInput = _fixture.GetValidInput(videoId: video.Id);
			_repositoryMock.Setup(x => x.Get(
				It.Is<Guid>(x => x == video.Id),
				It.IsAny<CancellationToken>())
			).ThrowsAsync(new NotFoundException("Video not found"));

			var action = () => _useCase.Handle(validInput, CancellationToken.None);

			await action.Should().ThrowAsync<NotFoundException>().WithMessage("Video not found");
		}


		[Fact(DisplayName = nameof(ClearStorageInUploadErrorCase))]
		[Trait("Application", "UploadMedias - Use Cases")]
		public async Task ClearStorageInUploadErrorCase()
		{
			var video = _fixture.GetValidVideo();
			var validInput = _fixture.GetValidInput(videoId: video.Id);
			var videoFileName = StorageFileName.Create(video.Id, nameof(video.Media), validInput.VideoFile!.Extension);
			var trailerFileName = StorageFileName.Create(video.Id, nameof(video.Trailer), validInput.TrailerFile!.Extension);
			var videoStoragePath = $"storage/{videoFileName}";
			var trailerStoragePath = $"storage/{trailerFileName}";
			var fileNames = new List<string>() { videoFileName, trailerFileName };
			_repositoryMock.Setup(x => x.Get(
				It.Is<Guid>(x => x == video.Id),
				It.IsAny<CancellationToken>())
			).ReturnsAsync(video);
			_storageServiceMock
				.Setup(x => x.Upload(
					It.Is<string>(x => x == videoFileName),
					It.IsAny<Stream>(),
					It.IsAny<CancellationToken>())
				).ReturnsAsync(videoStoragePath);
			_storageServiceMock
				.Setup(x => x.Upload(
					It.Is<string>(x => x == trailerFileName),
					It.IsAny<Stream>(),
					It.IsAny<CancellationToken>())
				).ThrowsAsync(new Exception("Something went wrong with the upload"));

			var action = () => _useCase.Handle(validInput, CancellationToken.None);

			await action.Should().ThrowAsync<Exception>()
				.WithMessage("Something went wrong with the upload");

			_repositoryMock.VerifyAll();
			_storageServiceMock.Verify(x =>
				x.Upload(
					It.Is<string>(x => fileNames.Contains(x)),
					It.IsAny<Stream>(),
					It.IsAny<CancellationToken>()),
				Times.Exactly(2)
			);
			_storageServiceMock.Verify(x =>
				x.Delete(
					It.Is<string>(fileName => fileName == videoStoragePath),
					It.IsAny<CancellationToken>()
				), Times.Exactly(1));
			_storageServiceMock.Verify(x =>
				x.Delete(It.IsAny<string>(), It.IsAny<CancellationToken>()),
					Times.Exactly(1));
		}
	}
}
