using CodeFlix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Moq;
using System.Runtime.CompilerServices;
using Xunit;
using UseCase = CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace CodeFlix.Catalog.UnitTests.Application.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName ="")]
        [Trait("Application", "UpdateCategory - Use Cases")]
        public async Task UpdateCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var category = _fixture.GetCategory();
            repositoryMock.Setup(x => x.Get(
                category.Id, 
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(category);
            var input = new UseCase.UpdateCategoryInput(
                category.Id,
                _fixture.GetValidCategoryName(),
                _fixture.GetValidCategoryDescription(),
                !category.IsActive
            );
            var useCase = new UseCase.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            repositoryMock.Verify(x=> x.Get(
                  category.Id,
                It.IsAny<CancellationToken>())
                , Times.Once);

            repositoryMock.Verify(x => x.Update(
                category,
                It.IsAny<CancellationToken>())
            , Times.Once);
            unitOfWorkMock.Verify(x => x.Commit(
                It.IsAny<CancellationToken>())
            ,Times.Once);
        }
    }
}
