﻿using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.UnitTests.Common;
using MyFlix.Catalog.Domain.Enum;
using MyFlix.Catalog.Application.UseCases.Video.Common;
using System.Text;
using MyFlix.Catalog.Application.UseCases.Video.CreateVideo;

namespace MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures
{
	public abstract class VideoTestFixtureBase : BaseFixture
	{

		public DomainEntity.Video GetValidVideo() => new DomainEntity.Video(
			GetValidTitle(),
			GetValidDescription(),
			GetValidYearLaunched(),
			GetRandomBoolean(),
			GetRandomBoolean(),
			GetValidDuration(),
			GetRandomRating()
		);

		public Rating GetRandomRating()
		{
			var enumValue = Enum.GetValues<Rating>();
			var random = new Random();
			return enumValue[random.Next(enumValue.Length)];
		}

		public string GetValidTitle()
			=> Faker.Lorem.Letter(100);

		public string GetValidDescription()
			=> Faker.Commerce.ProductDescription();

		public string GetTooLongDescription()
			=> Faker.Lorem.Letter(4001);

		public int GetValidYearLaunched()
			=> Faker.Date.BetweenDateOnly(
				new DateOnly(1960, 1, 1),
				new DateOnly(2022, 1, 1)
			).Year;

		public int GetValidDuration()
			=> (new Random()).Next(100, 300);

		public string GetTooLongTitle()
			=> Faker.Lorem.Letter(400);

		public string GetValidImagePath()
			=> Faker.Image.PlaceImgUrl();

		public FileInput GetValidImageFileInput()
		{
			var exampleStream = new MemoryStream(Encoding.ASCII.GetBytes("test"));
			var fileInput = new FileInput("jpg", exampleStream);
			return fileInput;
		}

		public string GetValidMediaPath()
		{
			var exampleMedias = new string[]
			{
			"https://www.googlestorage.com/file-example.mp4",
			"https://www.storage.com/another-example-of-video.mp4",
			"https://www.S3.com.br/example.mp4",
			"https://www.glg.io/file.mp4"
			};
			var random = new Random();
			return exampleMedias[random.Next(exampleMedias.Length)];
		}

		public FileInput GetValidMediaFileInput()
		{
			var exampleStream = new MemoryStream(Encoding.ASCII.GetBytes("test"));
			var fileInput = new FileInput("mp4", exampleStream);
			return fileInput;
		}

		public DomainEntity.Media GetValidMedia()
			=> new(GetValidMediaPath());

		internal CreateVideoInput CreateValidInputWithAllMedias() => new(
			 GetValidTitle(),
			 GetValidDescription(),
			 GetValidYearLaunched(),
			 GetRandomBoolean(),
			 GetRandomBoolean(),
			 GetValidDuration(),
			 GetRandomRating(),
			 null,
			 null,
			 null,
			 null,
			 null,
			 null,
			 GetValidMediaFileInput(),
			 GetValidMediaFileInput()
		 );

		public DomainEntity.Video GetValidVideoWithAllProperties()
		{
			var video = new DomainEntity.Video(
				GetValidTitle(),
				GetValidDescription(),
				GetValidYearLaunched(),
				GetRandomBoolean(),
				GetRandomBoolean(),
				GetValidDuration(),
				GetRandomRating()
			);

			video.UpdateBanner(GetValidImagePath());
			video.UpdateThumb(GetValidImagePath());
			video.UpdateThumbHalf(GetValidImagePath());

			video.UpdateMedia(GetValidMediaPath());
			video.UpdateTrailer(GetValidMediaPath());

			var random = new Random();
			Enumerable.Range(1, random.Next(2, 5)).ToList()
				.ForEach(_ => video.AddCastMember(Guid.NewGuid()));
			Enumerable.Range(1, random.Next(2, 5)).ToList()
				.ForEach(_ => video.AddCategory(Guid.NewGuid()));
			Enumerable.Range(1, random.Next(2, 5)).ToList()
				.ForEach(_ => video.AddGenre(Guid.NewGuid()));

			return video;
		}
	}
}
