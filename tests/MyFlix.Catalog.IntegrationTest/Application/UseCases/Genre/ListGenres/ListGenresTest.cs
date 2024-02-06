using FluentAssertions;
using MyFlix.Catalog.Application.UseCases.Genre.ListGenres;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using MyFlix.Catalog.Infra.Data.EF.Models;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using UseCase = MyFlix.Catalog.Application.UseCases.Genre.ListGenres;
namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.Genre.ListGenres
{
    [Collection(nameof(ListGenresTestFixture))]
    public class ListGenresTest
    {
        private readonly ListGenresTestFixture _fixture;

        public ListGenresTest(ListGenresTestFixture fixture)=> _fixture = fixture;

        [Fact(DisplayName = nameof(ListGenres))]
        [Trait("Integration/Application", "ListGenres - UseCases")]
        public async Task ListGenres()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var arrangeDbContext = _fixture.CreateDbContext();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var useCase = new UseCase.ListGenres(
                new GenreRepository(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var input = new UseCase.ListGenresInput(1, 20);

            ListGenresOutput output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleGenres.Count);
            output.Items.Should().HaveCount(exampleGenres.Count);
            output.Items.ToList().ForEach(outputItem => { 
                DomainEntity.Genre? exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
            });
        }

        [Fact(DisplayName = nameof(ListGenresReturnsEmptyWhenPersistenceIsEmpty))]
        [Trait("Integration/Application", "ListGenres - UseCases")]
        public async Task ListGenresReturnsEmptyWhenPersistenceIsEmpty()
        {
            var actDbContext = _fixture.CreateDbContext();
            var useCase = new UseCase.ListGenres(
                new GenreRepository(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var input = new UseCase.ListGenresInput(1, 20);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(0);
            output.Items.Should().HaveCount(0);
        }

        [Fact(DisplayName = nameof(ListGenresVerifyRelations))]
        [Trait("Integration/Application", "ListGenres - UseCases")]
        public async Task ListGenresVerifyRelations()
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            var random = new Random();
            exampleGenres.ForEach(genre =>
            {
                int relationsCount = random.Next(0, 3);
                for (int i = 0; i < relationsCount; i++)
                {
                    int selectedCategoryIndex = random.Next(0, exampleCategories.Count - 1);
                    var selected = exampleCategories[selectedCategoryIndex];
                    if (!genre.Categories.Contains(selected.Id))
                        genre.AddCategory(selected.Id);
                }
            });
            var genresCategories = new List<GenresCategories>();
            exampleGenres.ForEach(
                genre => genre.Categories.ToList().ForEach(
                    categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id))
                )
            );
            var arrangeDbContext = _fixture.CreateDbContext();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.AddRangeAsync(exampleCategories);
            await arrangeDbContext.AddRangeAsync(genresCategories);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var useCase = new UseCase.ListGenres(
                new GenreRepository(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var input = new UseCase.ListGenresInput(1, 20);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleGenres.Count);
            output.Items.Should().HaveCount(exampleGenres.Count);
            output.Items.ToList().ForEach(outputItem => {
                var exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                IReadOnlyList<Guid> outputItemCategoryIds = outputItem.Categories.Select(x => x.Id).ToList();
                outputItemCategoryIds.Should().BeEquivalentTo(exampleItem.Categories);
            });
        }

        [Theory(DisplayName = nameof(ListGenresPaginated))]
        [Trait("Integration/Application", "ListGenres - UseCases")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(10, 2, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        [InlineData(7, 3, 5, 0)]
        public async Task ListGenresPaginated(int quantityToGenerate, int page, int perPage, int expectedQuantityItems)
        {
            var exampleGenres = _fixture.GetExampleListGenres(quantityToGenerate);
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            var random = new Random();
            exampleGenres.ForEach(genre =>
            {
                var relationsCount = random.Next(0, 3);
                for (int i = 0; i < relationsCount; i++)
                {
                    var selectedCategoryIndex = random.Next(0, exampleCategories.Count - 1);
                    var selected = exampleCategories[selectedCategoryIndex];
                    if (!genre.Categories.Contains(selected.Id))
                        genre.AddCategory(selected.Id);
                }
            });
            var genresCategories = new List<GenresCategories>();
            exampleGenres.ForEach(genre => genre.Categories.ToList().ForEach(
                    categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id))
                )
            );
            var arrangeDbContext = _fixture.CreateDbContext();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.AddRangeAsync(exampleCategories);
            await arrangeDbContext.AddRangeAsync(genresCategories);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            UseCase.ListGenres useCase = new UseCase.ListGenres(
                new GenreRepository(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var input = new UseCase.ListGenresInput(page, perPage);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleGenres.Count);
            output.Items.Should().HaveCount(expectedQuantityItems);
            output.Items.ToList().ForEach(outputItem => {
                var exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                List<Guid> outputItemCategoryIds = outputItem.Categories.Select(x => x.Id).ToList();
                outputItemCategoryIds.Should().BeEquivalentTo(exampleItem.Categories);
                outputItem.Categories.ToList().ForEach(outputCategory =>
                {
                    var exampleCategory = exampleCategories.Find(x => x.Id == outputCategory.Id);
                    exampleCategory.Should().NotBeNull();
                    outputCategory.Name.Should().Be(exampleCategory!.Name);
                });
            });
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("Integration/Application", "ListGenres - UseCases")]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Horror", 2, 5, 0, 3)]
        [InlineData("Sci-fi", 1, 5, 4, 4)]
        [InlineData("Sci-fi", 1, 2, 2, 4)]
        [InlineData("Sci-fi", 2, 3, 1, 4)]
        [InlineData("Sci-fi Other", 1, 3, 0, 0)]
        [InlineData("Robots", 1, 5, 2, 2)]
        public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityTotalItems)
        {
            var exampleGenres = _fixture.GetExampleListGenresByNames(
                new List<string>() {
                "Action",
                "Horror",
                "Horror - Robots",
                "Horror - Based on Real Facts",
                "Drama",
                "Sci-fi IA",
                "Sci-fi Space",
                "Sci-fi Robots",
                "Sci-fi Future"
                }
            );
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            var random = new Random();
            exampleGenres.ForEach(genre =>
            {
                var relationsCount = random.Next(0, 3);
                for (int i = 0; i < relationsCount; i++)
                {
                    var selectedCategoryIndex = random.Next(0, exampleCategories.Count - 1);
                    var selected = exampleCategories[selectedCategoryIndex];
                    if (!genre.Categories.Contains(selected.Id))
                        genre.AddCategory(selected.Id);
                }
            });
            var genresCategories = new List<GenresCategories>();
            exampleGenres.ForEach(
                genre => genre.Categories.ToList().ForEach(
                    categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id))
                )
            );
            var arrangeDbContext = _fixture.CreateDbContext();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.AddRangeAsync(exampleCategories);
            await arrangeDbContext.AddRangeAsync(genresCategories);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var useCase = new UseCase.ListGenres(
                new GenreRepository(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var input = new UseCase.ListGenresInput(page, perPage, search);

            var output = await useCase.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(expectedQuantityTotalItems);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);
            output.Items.ToList().ForEach(outputItem => {
                var exampleItem = exampleGenres.Find(example => example.Id == outputItem.Id);
                outputItem.Name.Should().Contain(search);
                exampleItem.Should().NotBeNull();
                outputItem.Name.Should().Be(exampleItem!.Name);
                outputItem.IsActive.Should().Be(exampleItem.IsActive);
                List<Guid> outputItemCategoryIds = outputItem.Categories.Select(x => x.Id).ToList();
                outputItemCategoryIds.Should().BeEquivalentTo(exampleItem.Categories);
                outputItem.Categories.ToList().ForEach(outputCategory =>
                {
                    var exampleCategory = exampleCategories.Find(x => x.Id == outputCategory.Id);
                    exampleCategory.Should().NotBeNull();
                    outputCategory.Name.Should().Be(exampleCategory!.Name);
                });
            });
        }

        [Theory(DisplayName = nameof(Ordered))]
        [Trait("Integration/Application", "ListGenres - UseCases")]
        [InlineData("name", "asc")]
        [InlineData("name", "desc")]
        [InlineData("id", "asc")]
        [InlineData("id", "desc")]
        [InlineData("createdAt", "asc")]
        [InlineData("createdAt", "desc")]
        [InlineData("", "asc")]
        public async Task Ordered(
        string orderBy,
        string order
    )
        {
            var exampleGenres = _fixture.GetExampleListGenres(10);
            var exampleCategories = _fixture.GetExampleCategoriesList(10);
            var random = new Random();
            exampleGenres.ForEach(genre =>
            {
                var relationsCount = random.Next(0, 3);
                for (int i = 0; i < relationsCount; i++)
                {
                    var selectedCategoryIndex = random.Next(0, exampleCategories.Count - 1);
                    var selected = exampleCategories[selectedCategoryIndex];
                    if (!genre.Categories.Contains(selected.Id))
                        genre.AddCategory(selected.Id);
                }
            });
            var genresCategories = new List<GenresCategories>();
            exampleGenres.ForEach(
                genre => genre.Categories.ToList().ForEach(
                    categoryId => genresCategories.Add(new GenresCategories(categoryId, genre.Id))
                )
            );
            var arrangeDbContext = _fixture.CreateDbContext();
            await arrangeDbContext.AddRangeAsync(exampleGenres);
            await arrangeDbContext.AddRangeAsync(exampleCategories);
            await arrangeDbContext.AddRangeAsync(genresCategories);
            await arrangeDbContext.SaveChangesAsync();
            var actDbContext = _fixture.CreateDbContext(true);
            var useCase = new UseCase.ListGenres(
                new GenreRepository(actDbContext),
                new CategoryRepository(actDbContext)
            );
            var orderEnum = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
            var input = new UseCase.ListGenresInput(
                1, 20, sort: orderBy, dir: orderEnum
            );

            ListGenresOutput output = await useCase.Handle(input,CancellationToken.None);

            output.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(exampleGenres.Count);
            output.Items.Should().HaveCount(exampleGenres.Count);
            var expectedOrderedList = _fixture.CloneGenreListOrdered(
                exampleGenres, orderBy, orderEnum
            );
            for (int indice = 0; indice < expectedOrderedList.Count; indice++)
            {
                var expectedItem = expectedOrderedList[indice];
                var outputItem = output.Items[indice];
                expectedItem.Should().NotBeNull();
                outputItem.Name.Should().Be(expectedItem!.Name);
                outputItem.IsActive.Should().Be(expectedItem.IsActive);
                List<Guid> outputItemCategoryIds = outputItem.Categories.Select(x => x.Id).ToList();
                outputItemCategoryIds.Should().BeEquivalentTo(expectedItem.Categories);
                outputItem.Categories.ToList().ForEach(outputCategory =>
                {
                    var exampleCategory = exampleCategories.Find(x => x.Id == outputCategory.Id);
                    exampleCategory.Should().NotBeNull();
                    outputCategory.Name.Should().Be(exampleCategory!.Name);
                });
            }
        }
    }
}
