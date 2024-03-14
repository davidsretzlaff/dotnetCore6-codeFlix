﻿using Moq;
using MyFlix.Catalog.Application.Interfaces;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.Video.CreateVideo;
using DomainEntities = MyFlix.Catalog.Domain.Entity;
using FluentAssertions;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Domain.Exceptions;
using MyFlix.Catalog.Application.UseCases.Video.CreateVideo;
using MyFlix.Catalog.Application.Exceptions;

namespace MyFlix.Catalog.UnitTests.Application.Video.CreateVideo
{
	[Collection(nameof(CreateVideoTestFixture))]
	public class CreateVideoTest
	{
		private readonly CreateVideoTestFixture _fixture;

		public CreateVideoTest(CreateVideoTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(CreateVideo))]
		[Trait("Application", "CreateVideo - Use Cases")]
		public async Task CreateVideo()
		{
			var repositoryMock = new Mock<IVideoRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var useCase = new UseCase.CreateVideo(repositoryMock.Object, unitOfWorkMock.Object);
			var input = _fixture.CreateValidCreateVideoInput();

			var output = await useCase.Handle(input, CancellationToken.None);

			repositoryMock.Verify(x => x.Insert(
				It.Is<DomainEntities.Video>(
					video =>
						video.Title == input.Title &&
						video.Published == input.Published &&
						video.Description == input.Description &&
						video.Duration == input.Duration &&
						video.Rating == input.Rating &&
						video.Id != Guid.Empty &&
						video.YearLaunched == input.YearLaunched &&
						video.Opened == input.Opened
				),
				It.IsAny<CancellationToken>())
			);
			unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
			output.Id.Should().NotBeEmpty();
			output.CreatedAt.Should().NotBe(default(DateTime));
			output.Title.Should().Be(input.Title);
			output.Published.Should().Be(input.Published);
			output.Description.Should().Be(input.Description);
			output.Duration.Should().Be(input.Duration);
			output.Rating.Should().Be(input.Rating);
			output.YearLaunched.Should().Be(input.YearLaunched);
			output.Opened.Should().Be(input.Opened);
		}

		[Fact(DisplayName = nameof(CreateVideoWithCategoriesIds))]
		[Trait("Application", "CreateVideo - Use Cases")]
		public async Task CreateVideoWithCategoriesIds()
		{
			var repositoryMock = new Mock<IVideoRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var useCase = new UseCase.CreateVideo(repositoryMock.Object, unitOfWorkMock.Object);
			var examplecategoriesIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToList();
			var input = _fixture.CreateValidCreateVideoInput(examplecategoriesIds);

			var output = await useCase.Handle(input, CancellationToken.None);

			unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()));
			output.Id.Should().NotBeEmpty();
			output.CreatedAt.Should().NotBe(default(DateTime));
			output.Title.Should().Be(input.Title);
			output.Published.Should().Be(input.Published);
			output.Description.Should().Be(input.Description);
			output.Duration.Should().Be(input.Duration);
			output.Rating.Should().Be(input.Rating);
			output.YearLaunched.Should().Be(input.YearLaunched);
			output.Opened.Should().Be(input.Opened);
			output.CategoriesIds.Should().BeEquivalentTo(examplecategoriesIds);
			repositoryMock.Verify(x => x.Insert(
			It.Is<DomainEntities.Video>(
				video =>
					video.Title == input.Title &&
					video.Published == input.Published &&
					video.Description == input.Description &&
					video.Duration == input.Duration &&
					video.Rating == input.Rating &&
					video.Id != Guid.Empty &&
					video.YearLaunched == input.YearLaunched &&
					video.Opened == input.Opened &&
					video.Categories.All(categoryId => examplecategoriesIds.Contains(categoryId))
				),
				It.IsAny<CancellationToken>())
			);
		}

		[Fact(DisplayName = nameof(ThrowsWhenCategoryIdInvalid))]
		[Trait("Application", "CreateVideo - Use Cases")]
		public async Task ThrowsWhenCategoryIdInvalid()
		{
			var videoRepositoryMock = new Mock<IVideoRepository>();
			var categoryRepositoryMock = new Mock<ICategoryRepository>();
			var examplecategoriesIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToList();
			var removedcategoryId = examplecategoriesIds[2];
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			categoryRepositoryMock.Setup(x => x.GetIdsListByIds(
				It.IsAny<List<Guid>>(),
				It.IsAny<CancellationToken>())
			).ReturnsAsync(examplecategoriesIds.FindAll(x => x != removedcategoryId).AsReadOnly());
			var useCase = new UseCase.CreateVideo(videoRepositoryMock.Object, categoryRepositoryMock.Object,unitOfWorkMock.Object);
			var input = _fixture.CreateValidCreateVideoInput(examplecategoriesIds);

			var action = () => useCase.Handle(input, CancellationToken.None);
			await action.Should().ThrowAsync<RelatedAggregateException>().WithMessage($"Related category id (or ids) not found: {removedcategoryId}.");
		}

		[Theory(DisplayName = nameof(CreateVideoThrowsWithInvalidInput))]
		[Trait("Application", "CreateVideo - Use Cases")]
		[ClassData(typeof(CreateVideoTestDataGenerator))]
		public async Task CreateVideoThrowsWithInvalidInput(CreateVideoInput input, string expectedValidationError)
		{
			var repositoryMock = new Mock<IVideoRepository>();
			var unitOfWorkMock = new Mock<IUnitOfWork>();
			var useCase = new UseCase.CreateVideo(repositoryMock.Object, unitOfWorkMock.Object);

			var action = async () => await useCase.Handle(input, CancellationToken.None);

			var exceptionAssertion = await action.Should()
				.ThrowAsync<EntityValidationException>()
				.WithMessage($"There are validation errors");
			exceptionAssertion.Which.Errors!.ToList()[0].Message.Should().Be(expectedValidationError);
			repositoryMock.Verify(
				x => x.Insert(It.IsAny<DomainEntities.Video>(), It.IsAny<CancellationToken>()),
				Times.Never
			);
		}
	}
}
