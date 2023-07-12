using CodeFlix.Catalog.Application.UseCases.Category.Common;
using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using FluentAssertions;
using Moq;
using Xunit;
using UseCase = CodeFlix.Catalog.Application.UseCases.Category.ListCategories;

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
            var categoriesExampleList = _fixture.GetExampleCategoriesList();
            var input = new UseCase.ListCategoriesInput(
                page: 2,
                perPage: 15,
                search: "search-example",
                sort: "name",
                dir: SearchOrder.Asc
            );
            var outputRepositorySearch = new SearchOuput<Category>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<Category>)categoriesExampleList,
                total: categoriesExampleList.Count()
            );
            repositoryMock.Setup(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => 
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositorySearch);
            var useCase = new UseCase.ListCategories(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count());
            ((List<CategoryModelOutput>)output.Items).ForEach(outputItem =>
            {
                var repositoryCategory = outputRepositorySearch.Items.FirstOrDefault(x => x.Id == outputItem.Id);

                outputItem.Should().NotBeNull();
                outputItem.Name.Should().Be(repositoryCategory!.Name);
                outputItem.Description.Should().Be(repositoryCategory.Description);
                outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
                outputItem.Id.Should().Be(repositoryCategory.Id);
                outputItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
            });

            repositoryMock.Verify(x => x.Search(
                It.Is<SearchInput>(
                    searchInput => 
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
