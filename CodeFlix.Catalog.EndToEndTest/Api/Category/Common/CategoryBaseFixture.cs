using Bogus;
using CodeFlix.Catalog.EndToEndTest.Base;

namespace CodeFlix.Catalog.EndToEndTest.Api.Category.Common
{
    public class CategoryBaseFixture : BaseFixture
    {
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
            var categoryDescription =
                Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 10_000)
                categoryDescription =
                    categoryDescription[..10_000];
            return categoryDescription;
        }

        public bool getRandomBoolean()
            => new Random().NextDouble() < 0.5;

    }
}
