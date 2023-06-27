
using Xunit;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;
namespace CodeFlix.CatalogUnitTests.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Agregates")]
        public void Instantiate()
        {
            // Arrange
            var validDate = new 
            {
                Name = "Category name",
                Description = "Category Description"
            };

            // Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(category.Name,validDate.Name);
            Assert.Equal(category.Description, validDate.Description);
        }
    }
}
