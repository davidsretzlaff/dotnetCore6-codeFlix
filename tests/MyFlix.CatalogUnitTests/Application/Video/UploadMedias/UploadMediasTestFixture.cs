using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.Video.UploadMedias;

namespace MyFlix.Catalog.UnitTests.Application.Video.UploadMedias
{
    [CollectionDefinition(nameof(UploadMediasTestFixture))]
    public class UploadMediasTestFixtureCollection
    : ICollectionFixture<UploadMediasTestFixture>
    { }

    public class UploadMediasTestFixture : VideoTestFixtureBase
    {
        public UseCase.UploadMediasInput GetValidInput(Guid? videoId = null)
            => new(
				videoId ?? Guid.NewGuid(),
				GetValidMediaFileInput(),
                GetValidMediaFileInput()
            );
    }
}
