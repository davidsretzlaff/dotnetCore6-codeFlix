using CodeFlix.Catalog.Domain.Entity;
using CodeFlix.Catalog.Domain.Exceptions;
using System;
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
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description);
            var datetimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(category.Name, validDate.Name);
            Assert.Equal(category.Description, validDate.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Agregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            // Arrange
            var validDate = new
            {
                Name = "Category name",
                Description = "Category Description"
            };
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description,isActive);
            var datetimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(category.Name, validDate.Name);
            Assert.Equal(category.Description, validDate.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.Equal(category.IsActive, isActive);
        }
        [Theory(DisplayName =nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Agregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name) 
        {
            // Arrange
            Action action = () => new DomainEntity.Category(name!, "Category Description");

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Agregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            // Arrange
            Action action = () => new DomainEntity.Category("Category Name", null!);

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should not be empty or null", exception.Message);
        }
        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Agregates")]
        [InlineData("jo")]
        [InlineData("pe")]
        [InlineData("ju")]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            // Arrange
            Action action = () => new DomainEntity.Category(invalidName, "Category Description");

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be at least 3 characters", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Agregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            // Arrange
            var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            Action action = () => new DomainEntity.Category(invalidName, "Category Description");

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be less or equal 255 characters", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Agregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            // Arrange
            var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
            Action action = () => new DomainEntity.Category("Category Name", invalidDescription);

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Agregates")]
        public void Activate()
        {
            // Arrange
            var validDate = new
            {
                Name = "Category name",
                Description = "Category Description"
            };

            // Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description,false);
            category.Activate();

            // Assert            
            Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Agregates")]
        public void Deactivate()
        {
            // Arrange
            var validDate = new
            {
                Name = "Category name",
                Description = "Category Description"
            };

            // Act
            var category = new DomainEntity.Category(validDate.Name, validDate.Description, true);
            category.Deactivate();

            // Assert            
            Assert.False(category.IsActive);
        }
        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Agregates")]
        public void Update()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name", Description = "New Description" };

            category.Update(newValues.Name, newValues.Description);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Agregates")]
        public void UpdateOnlyName()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name" };
            var currentDescription = category.Description;

            category.Update(newValues.Name);

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
            var category = new DomainEntity.Category("Category Name", "Category Description");

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
            var category = new DomainEntity.Category("Category Name", "Category Description");

            // Act
            Action action = () => category.Update(invalidName);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be at least 3 characters", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Agregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            // Arrange
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            // Act
            Action action = () => category.Update(invalidName);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be less or equal 255 characters", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Agregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            // Arrange
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidDescription = string.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
            
            // Act
            Action action = () => category.Update("Category New Name", invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }


    }
}
