using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Exceptions;
using System;
using Xunit;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;
namespace CodeFlix.CatalogUnitTests.Domain.Entity.Category
{
    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTest
    {
        private readonly CategoryTestFixture _categoryTestFixture;
        public CategoryTest(CategoryTestFixture categoryTestFixture) => _categoryTestFixture = categoryTestFixture;

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Agregates")]
        public void Instantiate()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(category.Name, validCategory.Name);
            Assert.Equal(category.Description, validCategory.Description);
            Assert.NotEqual(default, category.Id);
            Assert.NotEqual(default, category.CreatedAt);
            Assert.True(category.CreatedAt >= datetimeBefore);
            Assert.True(category.CreatedAt <= datetimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Agregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(category.Name, validCategory.Name);
            Assert.Equal(category.Description, validCategory.Description);
            Assert.NotEqual(default, category.Id);
            Assert.NotEqual(default, category.CreatedAt);
            Assert.True(category.CreatedAt >= datetimeBefore);
            Assert.True(category.CreatedAt <= datetimeAfter);
            Assert.Equal(category.IsActive, isActive);
        }
        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Agregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => new DomainEntity.Category(name!, validCategory.Description);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Agregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => new DomainEntity.Category(validCategory.Name, null!);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should not be null", exception.Message);
        }
        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Agregates")]
        [InlineData("jo")]
        [InlineData("pe")]
        [InlineData("ju")]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();


            // Act
            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be less or equal 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Agregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            // Act
            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Agregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            // Act
            Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Agregates")]
        public void Activate()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);

            // Act
            category.Activate();

            // Assert            
            Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Agregates")]
        public void Deactivate()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);

            // Act
            category.Deactivate();

            // Assert            
            Assert.False(category.IsActive);
        }
        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Agregates")]
        public void Update()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            var newValues = new { Name = "New Name", Description = "New Description" };

            // Act
            category.Update(newValues.Name, newValues.Description);

            // Assert
            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Agregates")]
        public void UpdateOnlyName()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();
            var newValues = new { Name = "New Name" };
            var currentDescription = category.Description;

            // Act
            category.Update(newValues.Name);

            // Assert
            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(currentDescription, category.Description);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Agregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => category.Update(name!);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Agregates")]
        [InlineData("jo")]
        [InlineData("pe")]
        [InlineData("ju")]
        public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => category.Update(invalidName);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be less or equal 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Agregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();
            var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            // Act
            Action action = () => category.Update(invalidName);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Agregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();
            var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

            // Act
            Action action = () => category.Update("Category New Name", invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }
    }
}
