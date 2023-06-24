using Moq;
using Xunit;
using UseCases = CodeFlix.Catalog.Application.UseCases.CreateCategory;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;

namespace CodeFlix.CatalogUnitTests.Application.CreateCategory
{
    public class CreateCategoryTest
    {
        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var repositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IunitOfWorkRepository>();
            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object, 
                unitOfWorkMock.Object
            );

            var input = new CreateCategoryInput(
                "Category Name", 
                "Category Description", 
                true
            );
            
            var output = await useCase.Handle(input,CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<Category>(),
                    It.IsAny<CancellationToken>()
                 ),
                Times.Once
                );

            unitOfWorkMock.Verify(
               unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
               Times.Once
               );

            output.Should().NotBeNull();
            output.Name.Should().Be("Category Name");
            output.Description.Should().Be("Category Description");
            output.IsActive.Should().Be(true);
            (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
            (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();

        }
    }
}
