using Moq;
using Xunit;
using FluentAssertions;
using UseCase = MyFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using MyFlix.Catalog.Application.Exceptions;

namespace MyFlix.Catalog.UnitTests.Application.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "DeleteCategory")]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public async Task DeleteCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var categoryExample = _fixture.GetExampleCategory();
            repositoryMock.Setup(x => x.Get(
                categoryExample.Id,
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(categoryExample);
            var input = new UseCase.DeleteCategoryInput(categoryExample.Id);
            var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

            await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(x => x.Get(
                categoryExample.Id,
                It.IsAny<CancellationToken>()
            ), Times.Once);
            repositoryMock.Verify(x => x.Delete(
                categoryExample,
                It.IsAny<CancellationToken>()
            ), Times.Once);
            unitOfWorkMock.Verify(x => x.Commit(
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact(DisplayName = "ThrowWhenCategoryNotFound")]
        [Trait("Application", "ThrowWhenCategoryNotFound - Use Cases")]
        public async Task ThrowWhenCategoryNotFound()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var exampleGuid = Guid.NewGuid();
            repositoryMock.Setup(x => x.Get(
                exampleGuid,
                It.IsAny<CancellationToken>())
            ).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found."));
            var input = new UseCase.DeleteCategoryInput(exampleGuid);
            var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>();

            repositoryMock.Verify(x => x.Get(
                exampleGuid,
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
