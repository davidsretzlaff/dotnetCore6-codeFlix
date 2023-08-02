
using CodeFlix.Catalog.Application.UseCases.Category.CreateCategory;
using CodeFlix.Catalog.EndToEndTest.Api.Category.Common;
using Xunit;

namespace CodeFlix.Catalog.EndToEndTest.Api.Category.CreateCategory
{

    [CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTestFixtureCollection
        : ICollectionFixture<CreateCategoryApiTestFixture>
    { }

    public class CreateCategoryApiTestFixture
        : CategoryBaseFixture
    {
        public CreateCategoryInput getExampleInput()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                getRandomBoolean()
            );
    }
}
