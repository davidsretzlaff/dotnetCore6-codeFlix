using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.UnitTests.Entity.Category
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
            Assert.Equal(category.Name, validDate.Name);
            Assert.Equal(category.Description, validDate.Description);
        }
    }
}
