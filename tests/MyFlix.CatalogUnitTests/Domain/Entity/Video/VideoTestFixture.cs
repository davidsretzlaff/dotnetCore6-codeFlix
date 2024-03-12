﻿
using MyFlix.Catalog.Domain.Enum;
using MyFlix.Catalog.UnitTests.Common;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.Video
{
	[CollectionDefinition(nameof(VideoTestFixture))]
	public class VideoTestFixtureCollection : ICollectionFixture<VideoTestFixture>
	{ }

	public class VideoTestFixture : BaseFixture
	{
		public DomainEntity.Video GetValidVideo() 
			=> new DomainEntity.Video(
				GetValidTitle(),
				GetValidDescription(),
				GetValidYearLaunched(),
				GetRandomBoolean(),
				GetRandomBoolean(),
				GetValidDuration(),
				GetRandomRating()
			);

		public string GetValidTitle() => Faker.Lorem.Letter(100);

		public string GetValidDescription() => Faker.Commerce.ProductDescription();

		public int GetValidYearLaunched()
			=> Faker.Date.BetweenDateOnly(
				new DateOnly(1960, 1, 1),
				new DateOnly(2022, 1, 1)
			).Year;

		public int GetValidDuration() => (new Random()).Next(100, 300);

		public string GetTooLongTitle() => Faker.Lorem.Letter(400);

		public string GetTooLongDescription() => Faker.Lorem.Letter(4001);

		public Rating GetRandomRating()
		{
			var enumValue = Enum.GetValues<Rating>();
			var random = new Random();
			return enumValue[random.Next(enumValue.Length)];
		}
	}
}
