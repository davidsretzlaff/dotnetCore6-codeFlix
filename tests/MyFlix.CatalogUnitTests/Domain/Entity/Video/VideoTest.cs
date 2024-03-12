﻿
using Xunit;
using FluentAssertions;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Exceptions;
using MyFlix.Catalog.Domain.Validation;
using MediatR;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.Video
{
	[Collection(nameof(VideoTestFixture))]
	public class VideoTest
	{
		private readonly VideoTestFixture _fixture;

		public VideoTest(VideoTestFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Instantiate))]
		[Trait("Domain", "Video - Aggregate")]
		public void Instantiate()
		{
			var expectedTitle = _fixture.GetValidTitle();
			var expectedDescription = _fixture.GetValidDescription();
			var expectedYearLaunched = _fixture.GetValidYearLaunched();
			var expectedOpened = _fixture.GetRandomBoolean();
			var expectedPublished = _fixture.GetRandomBoolean();
			var expectedDuration = _fixture.GetValidDuration();
			var expectedRating = _fixture.GetRandomRating();

			var expectedCreatedDate = DateTime.Now;
			var video = new DomainEntity.Video(
				expectedTitle,
				expectedDescription,
				expectedYearLaunched,
				expectedOpened,
				expectedPublished,
				expectedDuration,
				expectedRating
			);

			video.Title.Should().Be(expectedTitle);
			video.Description.Should().Be(expectedDescription);
			video.YearLaunched.Should().Be(expectedYearLaunched);
			video.Opened.Should().Be(expectedOpened);
			video.Published.Should().Be(expectedPublished);
			video.Duration.Should().Be(expectedDuration);
			video.CreatedAt.Should().BeCloseTo(expectedCreatedDate, TimeSpan.FromSeconds(10)); 
		}

		[Fact(DisplayName = nameof(ValidateWhenValidState))]
		[Trait("Domain", "Video - Aggregate")]
		public void ValidateWhenValidState()
		{
			var validVideo = _fixture.GetValidVideo();
			var notificationHandler = new NotificationValidationHandler();

			validVideo.Validate(notificationHandler);

			notificationHandler.HasErrors().Should().BeFalse();
		}

		[Fact(DisplayName = nameof(ValidateWithErrorWhenInvalidState))]
		[Trait("Domain", "Video - Aggregate")]
		public void ValidateWithErrorWhenInvalidState()
		{
			var invalidVideo = new DomainEntity.Video(
				_fixture.GetTooLongTitle(),
				_fixture.GetTooLongDescription(),
				_fixture.GetValidYearLaunched(),
				_fixture.GetRandomBoolean(),
				_fixture.GetRandomBoolean(),
				_fixture.GetValidDuration(),
				_fixture.GetRandomRating()
			);

			var notificationHandler = new NotificationValidationHandler();

			invalidVideo.Validate(notificationHandler);

			notificationHandler.HasErrors().Should().BeTrue();
			notificationHandler.Errors.Should()
				.BeEquivalentTo(new List<ValidationError>()
				{
					new ValidationError("'Title' should be less or equal 255 characters long"),
					new ValidationError("'Description' should be less or equal 4000 characters long")
				});
		}

		[Fact(DisplayName = nameof(Update))]
		[Trait("Domain", "Video - Aggregate")]
		public void Update()
		{
			var expectedTitle = _fixture.GetValidTitle();
			var expectedDescription = _fixture.GetValidDescription();
			var expectedYearLaunched = _fixture.GetValidYearLaunched();
			var expectedOpened = _fixture.GetRandomBoolean();
			var expectedPublished = _fixture.GetRandomBoolean();
			var expectedDuration = _fixture.GetValidDuration();
			var video = _fixture.GetValidVideo();

			video.Update(
				expectedTitle,
				expectedDescription,
				expectedYearLaunched,
				expectedOpened,
				expectedPublished,
				expectedDuration
			);

			video.Title.Should().Be(expectedTitle);
			video.Description.Should().Be(expectedDescription);
			video.YearLaunched.Should().Be(expectedYearLaunched);
			video.Opened.Should().Be(expectedOpened);
			video.Published.Should().Be(expectedPublished);
			video.Duration.Should().Be(expectedDuration);
		}

		[Fact(DisplayName = nameof(ValidateStillValidatingAfterUpdateToValidState))]
		[Trait("Domain", "Video - Aggregate")]
		public void ValidateStillValidatingAfterUpdateToValidState()
		{
			var expectedTitle = _fixture.GetValidTitle();
			var expectedDescription = _fixture.GetValidDescription();
			var expectedYearLaunched = _fixture.GetValidYearLaunched();
			var expectedOpened = _fixture.GetRandomBoolean();
			var expectedPublished = _fixture.GetRandomBoolean();
			var expectedDuration = _fixture.GetValidDuration();
			var video = _fixture.GetValidVideo();
			video.Update(
				expectedTitle,
				expectedDescription,
				expectedYearLaunched,
				expectedOpened,
				expectedPublished,
				expectedDuration
			);
			var notificationHandler = new NotificationValidationHandler();

			video.Validate(notificationHandler);

			notificationHandler.HasErrors().Should().BeFalse();
		}

		[Fact(DisplayName = nameof(ValidateGenerateErrorsAfterUpdateToInvalidState))]
		[Trait("Domain", "Video - Aggregate")]
		public void ValidateGenerateErrorsAfterUpdateToInvalidState()
		{
			var expectedTitle = _fixture.GetTooLongTitle();
			var expectedDescription = _fixture.GetTooLongDescription();
			var expectedYearLaunched = _fixture.GetValidYearLaunched();
			var expectedOpened = _fixture.GetRandomBoolean();
			var expectedPublished = _fixture.GetRandomBoolean();
			var expectedDuration = _fixture.GetValidDuration();
			var video = _fixture.GetValidVideo();
			video.Update(
				expectedTitle,
				expectedDescription,
				expectedYearLaunched,
				expectedOpened,
				expectedPublished,
				expectedDuration
			);
			var notificationHandler = new NotificationValidationHandler();

			video.Validate(notificationHandler);

			notificationHandler.HasErrors().Should().BeTrue();
			notificationHandler.Errors.Should().HaveCount(2);
			notificationHandler.Errors.Should().BeEquivalentTo(
				new List<ValidationError>()
				{
					new ValidationError("'Title' should be less or equal 255 characters long"),
					new ValidationError("'Description' should be less or equal 4000 characters long")
				}
			);
		}
	}
}
