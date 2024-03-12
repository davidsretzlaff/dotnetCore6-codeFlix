﻿
using Xunit;
using FluentAssertions;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Exceptions;
using MyFlix.Catalog.Domain.Validation;
using MediatR;
using MyFlix.Catalog.Domain.Enum;

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
			video.Thumb.Should().BeNull();
			video.ThumbHalf.Should().BeNull();
			video.Banner.Should().BeNull();
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

		[Fact(DisplayName = nameof(UpdateThumb))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateThumb()
		{
			var validVideo = _fixture.GetValidVideo();
			var validImagePath = _fixture.GetValidImagePath();

			validVideo.UpdateThumb(validImagePath);

			validVideo.Thumb.Should().NotBeNull();
			validVideo.Thumb!.Path.Should().Be(validImagePath);
		}

		[Fact(DisplayName = nameof(UpdateThumbHalf))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateThumbHalf()
		{
			var validVideo = _fixture.GetValidVideo();
			var validImagePath = _fixture.GetValidImagePath();

			validVideo.UpdateThumbHalf(validImagePath);

			validVideo.ThumbHalf.Should().NotBeNull();
			validVideo.ThumbHalf!.Path.Should().Be(validImagePath);
		}

		[Fact(DisplayName = nameof(UpdateBanner))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateBanner()
		{
			var validVideo = _fixture.GetValidVideo();
			var validImagePath = _fixture.GetValidImagePath();

			validVideo.UpdateBanner(validImagePath);

			validVideo.Banner.Should().NotBeNull();
			validVideo.Banner!.Path.Should().Be(validImagePath);
		}

		[Fact(DisplayName = nameof(UpdateMedia))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateMedia()
		{
			var validVideo = _fixture.GetValidVideo();
			var validPath = _fixture.GetValidMediaPath();

			validVideo.UpdateMedia(validPath);

			validVideo.Media.Should().NotBeNull();
			validVideo.Media!.FilePath.Should().Be(validPath);
		}

		[Fact(DisplayName = nameof(UpdateTrailer))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateTrailer()
		{
			var validVideo = _fixture.GetValidVideo();
			var validPath = _fixture.GetValidMediaPath();

			validVideo.UpdateTrailer(validPath);

			validVideo.Trailer.Should().NotBeNull();
			validVideo.Trailer!.FilePath.Should().Be(validPath);
		}

		[Fact(DisplayName = nameof(UpdateAsSentToEncode))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateAsSentToEncode()
		{
			var validVideo = _fixture.GetValidVideo();
			var validPath = _fixture.GetValidMediaPath();
			validVideo.UpdateMedia(validPath);

			validVideo.UpdateAsSentToEncode();

			validVideo.Media!.Status.Should().Be(MediaStatus.Processing);
		}

		[Fact(DisplayName = nameof(UpdateAsSentToEncodeThrowsWhenThereIsNoMedia))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateAsSentToEncodeThrowsWhenThereIsNoMedia()
		{
			var validVideo = _fixture.GetValidVideo();

			var action = () => validVideo.UpdateAsSentToEncode();

			action.Should().Throw<EntityValidationException>()
				.WithMessage("There is no Media");
		}

		[Fact(DisplayName = nameof(UpdateAsEncoded))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateAsEncoded()
		{
			var validVideo = _fixture.GetValidVideo();
			var validPath = _fixture.GetValidMediaPath();
			var validEncodedPath = _fixture.GetValidMediaPath();
			validVideo.UpdateMedia(validPath);

			validVideo.UpdateAsEncoded(validEncodedPath);

			validVideo.Media!.Status.Should().Be(MediaStatus.Completed);
			validVideo.Media!.EncodedPath.Should().Be(validEncodedPath);
		}

		[Fact(DisplayName = nameof(UpdateAsEncodedThrowsWhenThereIsNoMedia))]
		[Trait("Domain", "Video - Aggregate")]
		public void UpdateAsEncodedThrowsWhenThereIsNoMedia()
		{
			var validVideo = _fixture.GetValidVideo();
			var validPath = _fixture.GetValidMediaPath();

			var action = () => validVideo.UpdateAsEncoded(validPath);

			action.Should().Throw<EntityValidationException>()
				.WithMessage("There is no Media");
		}

		[Fact(DisplayName = nameof(AddCategory))]
		[Trait("Domain", "Video - Aggregate")]
		public void AddCategory()
		{
			var validVideo = _fixture.GetValidVideo();
			var categoryIdExample = Guid.NewGuid();

			validVideo.AddCategory(categoryIdExample);

			validVideo.Categories.Should().HaveCount(1);
			validVideo.Categories[0].Should().Be(categoryIdExample);
		}

		[Fact(DisplayName = nameof(RemoveCategory))]
		[Trait("Domain", "Video - Aggregate")]
		public void RemoveCategory()
		{
			var validVideo = _fixture.GetValidVideo();
			var categoryIdExample = Guid.NewGuid();
			var categoryIdExample2 = Guid.NewGuid();
			validVideo.AddCategory(categoryIdExample);
			validVideo.AddCategory(categoryIdExample2);

			validVideo.Removecategory(categoryIdExample);

			validVideo.Categories.Should().HaveCount(1);
			validVideo.Categories[0].Should().Be(categoryIdExample2);
		}
	}
}
