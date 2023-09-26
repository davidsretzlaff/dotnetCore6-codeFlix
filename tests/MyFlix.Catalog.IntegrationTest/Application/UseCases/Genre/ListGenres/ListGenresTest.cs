using FluentAssertions;
using MyFlix.Catalog.Application.UseCases.Genre.ListGenres;
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
    }
}
