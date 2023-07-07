using FluentAssertions;
using Moq;
using Xunit;
using UseCase = CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
namespace CodeFlix.Catalog.UnitTests.Application.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        public GetCategoryTest(GetCategoryTestFixture fixture)
            =>_fixture = fixture;
        
        private readonly GetCategoryTestFixture _fixture;

        [Fact(DisplayName ="")]
        [Trait("Application", "GetCategory - Use Case")]
        public async Task GetCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var exampleCategory = _fixture.GetValidCategory();
            repositoryMock.Setup(x => x.Get(
                It.IsAny<Guid>(), 
                It.IsAny<CancellationToken>()
                )).ReturnsAsync(exampleCategory);

            var input = new UseCase.GetCategoryInput(exampleCategory.Id);
            var useCase = new UseCase.GetCategory(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);
            repositoryMock.Verify(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
                ),Times.Once);

            output.Should().NotBeNull();
            output.Name.Should().Be(exampleCategory.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));




        }
    }
}
