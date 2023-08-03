using MyFlix.Catalog.Application.UseCases.Category.Common;
using MyFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using MyFlix.Catalog.Infra.Data.EF;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using ApplicationUseCase = MyFlix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Domain.Exceptions;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Category.UpdateCategory
{

    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
            => _fixture = fixture;

        [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
           nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
           parameters: 5,
           MemberType = typeof(UpdateCategoryTestDataGenerator)
       )]
        public async Task UpdateCategoryWithoutIsActive(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var input = new UpdateCategoryInput(exampleCategory.Id,exampleCategory.Name, exampleCategory.Description);
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackinginfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            // Detached tracking EF
            trackinginfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);
            
            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be(output.Description);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt!);
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        }

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
           nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
           parameters: 5,
           MemberType = typeof(UpdateCategoryTestDataGenerator)
       )]
        public async Task UpdateCategory(DomainEntity.Category category, UpdateCategoryInput input)
        {
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackinginfo = await dbContext.AddAsync(category);
            dbContext.SaveChanges();
            // Detached tracking EF
            trackinginfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be(output.Description);
            dbCategory.IsActive.Should().Be((bool)output.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt!);
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);
        }

        [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
           nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
           parameters: 5,
           MemberType = typeof(UpdateCategoryTestDataGenerator)
       )]
        public async Task UpdateCategoryOnlyName(DomainEntity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name);
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackinginfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            // Detached tracking EF
            trackinginfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt!);
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be((bool)exampleCategory.IsActive!);
        }

        [Fact(DisplayName = nameof(UpdateThrowsWhenNotFoundCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        public async Task UpdateThrowsWhenNotFoundCategory()
        {
            var input = _fixture.GetValidInput();
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

            var task = async() => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Category '{input.Id}' not found.");
        }

        [Theory(DisplayName = nameof(UpdateThrowsWhenCantInstantiateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(
             nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
             parameters: 5,
             MemberType = typeof(UpdateCategoryTestDataGenerator)
         )]
        public async Task UpdateThrowsWhenCantInstantiateCategory(UpdateCategoryInput input, string expectedExceptionMessage)
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCategories = _fixture.GetExampleCategoriesList();
            await dbContext.AddRangeAsync(exampleCategories);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);
            input.Id = exampleCategories.First().Id;

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>()
                .WithMessage(expectedExceptionMessage);
        }
    }
}
