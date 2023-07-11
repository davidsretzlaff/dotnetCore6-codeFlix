using CodeFlix.Catalog.Domain.Entity;
using Moq;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.ListCategories
{
    [Collection(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesTestFixture _fixture;
        public ListCategoriesTest(ListCategoriesTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "List")]
        [Trait("Application", "ListCategories - Use Cases")]
        public async Task List()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var categoriesExampleList = _fixture.GetExampleCategoriesList()
            var input = new ListCategoriesInput(
                page: 2,
                perPage: 15,
                search: "search-example",
                sort: "name",
                dir: SearchOrder.Asc
            );
            var outputRepositorySearch = new OutputSearch<Category>(
                CurrentPage: input.page,
                PerPage: input.perPage,
                Items: (IReadOnlyList<Category>)categoriesExampleList,
                Total: categoriesExampleList.Count()
            );
            repositoryMock.Setup(x => x.Search(
                It.Is<Searchinput>(
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositorySearch);
            var useCase = new ListCategories(repositoryMock.Object);

            var output = await useCase.handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.currentPage);
            output.PerPage.Should().Be(outputRepositorySearch.perPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count());
            output.Items.Foreach(outputItem =>
            {
                var repositoryCategory = outputRepositorySearch.Items.Find(x => x.Id == outputItem.Id);

                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(repositoryCategory.Name);
                outputItem.Description.Should().Be(repositoryCategory.Description);
                outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
                outputItem.Id.Should().Be(repositoryCategory.Id);
                outputItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
            });

            repositoryMock.Verify(x => x.Search(
                It.Is<Searchinput>(
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);

        }
    }
}
