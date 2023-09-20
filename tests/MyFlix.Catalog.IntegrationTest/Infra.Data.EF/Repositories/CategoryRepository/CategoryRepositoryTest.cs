using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using MyFlix.Catalog.Infra.Data.EF;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;
using Repository = MyFlix.Catalog.Infra.Data.EF.Repositories;
namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CategoryRepository
{
    [Collection(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTest 
    {
        private readonly CategoryRepositoryTestFixture _fixture;

        public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName =nameof(Insert))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Insert()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            
            await categoryRepository.Insert(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            
            // creating new context because EF tracking
            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Get()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));
     
            var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

            dbCategory.Should().NotBeNull();
            dbCategory.Id.Should().Be(exampleCategory.Id);
            dbCategory!.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(GetThrowIfNotFound))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task GetThrowIfNotFound()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleId = Guid.NewGuid();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList(15));
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            var task = async () => await categoryRepository.Get(exampleId, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Category '{exampleId}' not found.");
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Update()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var newCategoryValues = _fixture.GetExampleCategory();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);

            await categoryRepository.Update(exampleCategory, CancellationToken.None);
            dbContext.SaveChanges();
            //Creating new dbcontext because of EF tracking
            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id );
            // it could be like this
            //var dbCategory = await dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == exampleCategory.Id);
            
            dbCategory.Should().NotBeNull();
            dbCategory!.Id.Should().Be(exampleCategory.Id);
            dbCategory.Name.Should().Be(exampleCategory.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(Delete))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Delete()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            exampleCategoriesList.Add(exampleCategory);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);

            await categoryRepository.Delete(exampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            
            //Creating new dbcontext because of EF tracking
            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(exampleCategory.Id);
            
            dbCategory.Should().BeNull();
        }
        
        [Fact(DisplayName = nameof(SearchReturnListAndTotal))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchReturnListAndTotal()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(1,20,"","",SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);
            
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);

            foreach(Category outputItem in output.Items)
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

        [Fact(DisplayName = nameof(SearchReturnEmptyWhenPersistenceIsEmpty))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchReturnEmptyWhenPersistenceIsEmpty()
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Theory(DisplayName = nameof(SearchReturnPaginated))]
        [InlineData(10,1,5,5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
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
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(quantityCategoriesToGenerate);
            output.Items.Should().HaveCount(expectedQuantityItems);

            foreach (Category outputItem in output.Items)
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
        [InlineData("Action",1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Horror", 2, 5, 0, 3)]
        [InlineData("Sci-fi", 1, 5, 4, 4)]
        [InlineData("Sci-fi", 1, 2, 2, 4)]
        [InlineData("Sci-fi", 2, 3, 1, 4)]
        [InlineData("Sci-fi Other", 1, 5, 0, 0)]
        [InlineData("Robots", 1, 5, 2, 2)]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchByText(
            string search,
            int page,
            int perPage,
            int expectedQuantityItemsReturned,
            int expectedQuantityTotalItems
        )
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(new List<string>()
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
            });
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (Category outputItem in output.Items)
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
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchOrdered(
            string order,
            string orderBy
        )
        {
            CatalogDbContext dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(10);
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var searchInput = new SearchInput(1, 20, "", orderBy, searchOrder);

            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            var expectedOrderedList = _fixture.CloneCategoriesListOrdered(
                exampleCategoriesList,
                orderBy,
                searchOrder);
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(exampleCategoriesList.Count);
            output.Items.Should().HaveCount(exampleCategoriesList.Count);
            for(int indice = 0; indice < expectedOrderedList.Count; indice++)
            {
                var expectedItem = expectedOrderedList[indice];
                var outputItem = output.Items[indice];
                outputItem.Should().NotBeNull();
                expectedItem.Should().NotBeNull();
                outputItem.Id.Should().Be(expectedItem!.Id);
                outputItem.Name.Should().Be(expectedItem.Name);
                outputItem.Description.Should().Be(expectedItem.Description);
                outputItem.IsActive.Should().Be(expectedItem.IsActive);
                outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);

            }
        }

        [Fact(DisplayName = nameof(ListByIds))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task ListByIds()
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(15);
            var categoriesIdsToGet = Enumerable.Range(1, 3).Select(_ => {
                int indexToGet = (new Random()).Next(0, exampleCategoriesList.Count - 1);
                return exampleCategoriesList[indexToGet].Id;
            }).Distinct().ToList();
            await dbContext.AddRangeAsync(exampleCategoriesList);
            await dbContext.SaveChangesAsync(CancellationToken.None);
            var categoryRepository = new Repository.CategoryRepository(dbContext);
            var categoriesList = await categoryRepository.ListByIds(categoryIdsToGet);

            categoriesList.Should().NotBeNull();
            categoriesList.Should().HaveCount(categoriesIdsToGet.Count);
            foreach (Category outputItem in categoriesList)
            {
                var exampleItem = exampleCategoriesList.Find(
                    category => category.Id == outputItem.Id
                );
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.Description.Should().Be(exampleItem.Description);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
            }
        }
    }
}
