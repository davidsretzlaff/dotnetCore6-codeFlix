﻿using MyFlix.Catalog.UnitTests.Application.Video.Common.Fixtures;
using Xunit;
using DomainEntities = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.UnitTests.Application.Video.ListVideos
{

	[CollectionDefinition(nameof(ListVideosTestFixture))]
	public class ListVideosTestFixtureCollection
		: ICollectionFixture<ListVideosTestFixture>
	{ }

	public class ListVideosTestFixture : VideoTestFixtureBase
	{
		public List<DomainEntities.Video> CreateExampleVideosList()
			=> Enumerable.Range(1, Random.Shared.Next(2, 10))
				.Select(_ => GetValidVideoWithAllProperties())
				.ToList();

		public string GetValidCategoryName()
		{
			var categoryName = "";
			while (categoryName.Length < 3)
				categoryName = Faker.Commerce.Categories(1)[0];
			if (categoryName.Length > 255)
				categoryName = categoryName[..255];
			return categoryName;
		}

		public string GetValidCategoryDescription()
		{
			var categoryDescription =Faker.Commerce.ProductDescription();
			if (categoryDescription.Length > 10_000)
				categoryDescription = categoryDescription[..10_000];
			return categoryDescription;
		}

		public DomainEntities.Category GetExampleCategory()
			=> new(
				GetValidCategoryName(),
				GetValidCategoryDescription(),
				GetRandomBoolean()
			);

		public (List<DomainEntities.Video> Videos,List<DomainEntities.Category> Categories) CreateExampleVideosListWithRelations()
		{
			var itemsQuantityToBeCreated = Random.Shared.Next(2, 10);
			List<DomainEntities.Category> categories = new();
			var videos = Enumerable.Range(1, itemsQuantityToBeCreated)
				.Select(_ => GetValidVideoWithAllProperties())
				.ToList();

			videos.ForEach(video =>
			{
				video.RemoveAllCategory();
				var qtdCategories = Random.Shared.Next(2, 5);
				for (var i = 0; i < qtdCategories; i++)
				{
					var category = GetExampleCategory();
					categories.Add(category);
					video.AddCategory(category.Id);
				}
			});

			return (videos, categories);
		}
	}
}
