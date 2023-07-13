using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.UnitTests.Application.Common;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection :
        ICollectionFixture<ListCategoriesTestFixture>
    { }
    public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
    {
        public List<Category> GetExampleCategoriesList(int lenght = 10)
        {
            var list = new List<Category>() { };
            for (int i = 0; i < lenght; i++)
                list.Add(GetExampleCategory());
            return list;

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
