using Moq;
using Xunit;
using UseCases = MyFlix.Catalog.Application.UseCases.Category.CreateCategory;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Application.UseCases.Category.CreateCategory;
using MyFlix.Catalog.Domain.Exceptions;
using FluentAssertions;

namespace MyFlix.Catalog.UnitTests.Application.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            var input = _fixture.GetInput();

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<DomainEntity.Category>(),
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
            Assert.Equal(input.IsActive, output.IsActive);
            Assert.NotEqual(default, output.Id);
            Assert.NotEqual(default, output.CreatedAt);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategoryWithOnlyName()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<DomainEntity.Category>(),
                    It.IsAny<CancellationToken>()
                 ),
                Times.Once
                );

            unitOfWorkMock.Verify(
               unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
               Times.Once
               );

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategoryWithOnlyNameAndDescription()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );

            var input = new CreateCategoryInput(_fixture.GetValidCategoryName(), _fixture.GetValidCategoryDescription());

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<DomainEntity.Category>(),
                    It.IsAny<CancellationToken>()
                 ),
                Times.Once
                );

            unitOfWorkMock.Verify(
               unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
               Times.Once
               );

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(nameof(GetInvalidInputs))]
        public async void ThrowWhenCantInstantiateAggregate(
            CreateCategoryInput input,
            string exceptionMessage
        )
        {
            var useCase = new UseCases.CreateCategory(
                _fixture.GetRepositoryMock().Object,
                _fixture.GetUnitOfWorkMock().Object
            );

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        }

        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateCategoryTestFixture();
            var invalidInputsList = new List<object[]>();

            var invalidInputsShortName = fixture.GetInput();
            invalidInputsShortName.Name =
                invalidInputsShortName.Name.Substring(0, 2);

            invalidInputsList.Add(new object[]
            {
                invalidInputsShortName,
                "Name should be at least 3 characters long"
            });

            // too long name
            var invalidInputsToolongName = fixture.GetInput();
            var toolongNameForCategory = fixture.Faker.Commerce.ProductName();

            while (toolongNameForCategory.Length <= 255)
            {
                toolongNameForCategory = $"{toolongNameForCategory} {fixture.Faker.Commerce.ProductName()}";
            }

            invalidInputsToolongName.Name = toolongNameForCategory;

            invalidInputsList.Add(new object[]
            {
                invalidInputsToolongName,
                "Name should be less or equal 255 characters long"
            });

            // description null
            var invalidInputDescriptionNull = fixture.GetInput();
            invalidInputDescriptionNull.Description = null!;

            invalidInputsList.Add(new object[]
            {
                invalidInputDescriptionNull,
                "Description should not be null"
            });

            //description too long
            var invalidInputsToolongDescription = fixture.GetInput();
            var toolongDescriptionForCategory = fixture.Faker.Commerce.ProductDescription();

            while (toolongDescriptionForCategory.Length <= 10001)
            {
                toolongDescriptionForCategory = $"{toolongDescriptionForCategory} {fixture.Faker.Commerce.ProductDescription()}";
            }

            invalidInputsToolongDescription.Description = toolongDescriptionForCategory;

            invalidInputsList.Add(new object[]
            {
                invalidInputsToolongDescription,
                "Description should be less or equal 10000 characters long"
            });
            return invalidInputsList;
        }
    }
}
