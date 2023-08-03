using MyFlix.Catalog.Application.UseCases.Category.ListCategories;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using MyFlix.Catalog.UnitTests.Application.Category.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTestFixtureCollection :
        ICollectionFixture<ListCategoriesTestFixture>
    { }
    public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
    {
        public List<DomainEntity.Category> GetExampleCategoriesList(int lenght = 10)
        {
            var list = new List<DomainEntity.Category>() { };
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
