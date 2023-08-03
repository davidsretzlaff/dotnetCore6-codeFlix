﻿using MyFlix.Catalog.Infra.Data.EF;
using FluentAssertions;
using Xunit;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.Application.UseCases.Category.ListCategories;
using ApplicationUseCases = MyFlix.Catalog.Application.UseCases.Category.ListCategories;
using MyFlix.Catalog.Application.UseCases.Category.Common;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using MyFlix.Catalog.Domain.SeedWork;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Category.ListCategory
{

    [Collection(nameof(ListCategoryTestFixture))]
    public class ListCategoryTest
    {
        private readonly ListCategoryTestFixture _fixture;

        public ListCategoryTest(ListCategoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(SearchReturnListAndTotal))]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        public async Task SearchReturnListAndTotal()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(1, 20);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);

            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(
                    category => category.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem!.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Fact(DisplayName = nameof(SearchReturnEmptyWhenEmpty))]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        public async Task SearchReturnEmptyWhenEmpty()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var categoryRepository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(1, 20);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = nameof(SearchReturnPaginated))]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        public async Task SearchReturnPaginated(
            int quantityCategoriesToGenerate,
            int page,
            int perPage,
            int expectedQuantityItems
        )
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(page, perPage);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(expectedQuantityItems);

            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(
                    category => category.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem!.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }


        [Theory(DisplayName = nameof(SearchByText))]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Horror", 2, 5, 0, 3)]
        [InlineData("Sci-fi", 1, 5, 4, 4)]
        [InlineData("Sci-fi", 1, 2, 2, 4)]
        [InlineData("Sci-fi", 2, 3, 1, 4)]
        [InlineData("Sci-fi Other", 1, 5, 0, 0)]
        [InlineData("Robots", 1, 5, 2, 2)]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        public async Task SearchByText(
            string search,
            int page,
            int perPage,
            int expectedQuantityItemsReturned,
            int expectedQuantityTotalItems
        )
        {
            var exampleNamesList = new List<string>()
            {
                "Action",
                "Horror",
                "Horror - Robots",
                "Horror - Based on Real Facts",
                "Drama",
                "Sci-fi IA",
                "Sci-fi Space",
                "Sci-fi Robots",
                "Sci-fi Future"
            };

            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(exampleNamesList);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(page, perPage,search);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (CategoryModelOutput outputItem in output.Items)
            {
                var exampleItem = exampleCategoriesList.Find(
                    category => category.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem!.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }

        [Theory(DisplayName = nameof(SearchOrdered))]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        [InlineData("createdat", "asc")]
        [InlineData("createdat", "desc")]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        public async Task SearchOrdered(
           string order,
           string orderBy
       )
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new CategoryRepository(dbContext);
            var useCaseOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new ListCategoriesInput(1, 20,"",orderBy, useCaseOrder);
            var useCase = new ApplicationUseCases.ListCategories(categoryRepository);

            var output = await useCase.Handle(input, CancellationToken.None);

            var expectedOrderedList = _fixture.CloneCategoriesListOrdered(exampleCategoriesList, input.Sort, input.Dir);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);

            for (int indice = 0; indice < expectedOrderedList.Count; indice++)
            {
                var outputItem = output.Items[indice];
                var exampleItem = expectedOrderedList[indice];

                exampleItem.Should().NotBeNull();
                outputItem.Should().NotBeNull();
                outputItem.Id.Should().Be(exampleItem!.Id);
                outputItem.Name.Should().Be(exampleItem.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }
    }
}
