using UseCase = CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using CodeFlix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using FluentAssertions;
using CodeFlix.Catalog.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CodeFlix.Catalog.IntegrationTest.Application.UseCases.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryTest(GetCategoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Application/Integration", "GetCategory - Use Case")]
        public async Task GetCategory()
        {
            var exampleCategory = _fixture.GetExampleCategory();
            var dbContext = _fixture.CreateDbContext();
            dbContext.Categories.Add(exampleCategory);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);

            var input = new UseCase.GetCategoryInput(exampleCategory.Id);
            var useCase = new UseCase.GetCategory(repository);

            var output = await useCase.Handle(input, CancellationToken.None);
            
            output.Should().NotBeNull();
            output.Name.Should().Be(exampleCategory.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
        [Trait("Application/Integration", "GetCategory - Use Cases")]
        public async Task NotFoundExceptionWhenCategoryDoesntExist()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList();
            var dbContext = _fixture.CreateDbContext();
            dbContext.Categories.AddRange(exampleCategoriesList);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var exampleGuid = Guid.NewGuid();
            
            var input = new UseCase.GetCategoryInput(exampleGuid);
            var useCase = new UseCase.GetCategory(repository);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Category '{exampleGuid}' not found.");
        }

    }
}
