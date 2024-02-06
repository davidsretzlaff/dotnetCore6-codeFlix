using MyFlix.Catalog.Application.UseCases.Category.CreateCategory;
using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Exceptions;
using MyFlix.Catalog.Infra.Data.EF;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;
using ApplicationUseCases = MyFlix.Catalog.Application.UseCases.Category.CreateCategory;
namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application/Integration", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(
                repository,
                unitOfWork
            );
            var input = _fixture.GetInput();
            
            var output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be(output.Description);
            dbCategory.IsActive.Should().Be(output.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Application/Application", "CreateCategory - Use Cases")]
        public async void CreateCategoryWithOnlyName()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(
                repository,
                unitOfWork
            );
            var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

            var output = await useCase.Handle(input, CancellationToken.None);
                
            var dbCategory = await (_fixture.CreateDbContext(true)).
                Categories.
                FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be("");
            dbCategory.IsActive.Should().BeTrue();
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Application/Integration", "CreateCategory - Use Cases")]
        public async void CreateCategoryWithOnlyNameAndDescription()
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(
                repository,
                unitOfWork
            );
            var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

            var output = await useCase.Handle(input, CancellationToken.None);

            var dbCategory = await (_fixture.CreateDbContext(true)).
                Categories.
                FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(output.Name);
            dbCategory.Description.Should().Be(output.Description);
            dbCategory.IsActive.Should().BeTrue();
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(
            nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
            parameters:6,
            MemberType = typeof(CreateCategoryTestDataGenerator)
        )]
        public async void ThrowWhenCantInstantiateCategory(
            CreateCategoryInput input,
            string expectedExceptionMessage
        )
        {
            var dbContext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCases.CreateCategory(
                repository, unitOfWork
            );

            var task = async () =>  await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);
          
        }
    }
}
