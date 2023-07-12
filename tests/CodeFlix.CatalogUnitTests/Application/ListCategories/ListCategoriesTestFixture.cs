using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Repository;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.UnitTests.Common;
using Moq;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection :
        ICollectionFixture<ListCategoriesTestFixture>
    { }
    public class ListCategoriesTestFixture : BaseFixture
    {
        public Mock<ICategoryRepository> GetRepositoryMock() => new();

        public Category GetExampleCategory()
        {
            return new
            (
                GetValidCategoryName(),
                GetValidCategoryDescription()
            );
        }
        public List<Category> GetExampleCategoriesList(int lenght = 10)
        {
            var list = new List<Category>() { };
            for (int i = 0; i < lenght; i++)
                list.Add(GetExampleCategory());
            return list;

        }

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
            var categoryDescription = Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];
            return categoryDescription; ;
        }

        public ListCategoriesInput GetExampleInput()
        {
            var random = new Random();

            return new ListCategoriesInput(
               page: random.Next(1, 10),
               perPage: random.Next(15, 100),
               search: Faker.Commerce.ProductName(),
               sort: Faker.Commerce.ProductName(),
               dir: random.Next(0, 15) > 5 ? SearchOrder.Asc : SearchOrder.Asc
           );
        }
    }
}
