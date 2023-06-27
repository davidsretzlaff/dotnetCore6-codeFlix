using Moq;
using Xunit;
using UseCases = CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.Application.Interfaces;

namespace CodeFlix.CatalogUnitTests.Application.CreateCategory
{
    public class CreateCategoryTest
    {
        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var repositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object, 
                unitOfWorkMock.Object
            );

            var input = new UseCases.CreateCategoryInput(
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


            Assert.NotNull(output);
            Assert.Equal(output.Name, input.Name);
            Assert.Equal(output.Description, input.Description);
            Assert.True(output.IsActive);
            Assert.NotEqual(default(Guid), output.Id);
            Assert.NotEqual(default(DateTime), output.CreatedAt);
            
        }
    }
}
