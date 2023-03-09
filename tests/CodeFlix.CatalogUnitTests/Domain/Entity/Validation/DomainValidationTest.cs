using CodeFlix.Catalog.Domain.Exceptions;
using CodeFlix.Catalog.Domain.Validation;
using CodeFlix.CatalogUnitTests.Domain.Entity.Category;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.ComponentModel;
using Xunit;

namespace CodeFlix.CatalogUnitTests.Domain.Entity.Validation
{
    public class DomainValidationTest
    {
        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            string argumentValue = "Category";

            var exception = Record.Exception(() => DomainValidation.NotNull(argumentValue, "Value"));
            
            Assert.Null(exception);      
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? argumentValue = null;

            Action action = () => DomainValidation.NotNull(argumentValue, "FieldName");

            //var exception = Record.Exception(() => DomainValidation.NotNull(argumentName!, argumentValue));
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("FieldName should not be null", exception.Message);
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void NotNullOrEmptyThrowWhenEmpty(string? target)
        {
            Action action = () => DomainValidation.NotNullOrEmpty(target, "fieldName");

            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("fieldName should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk()
        {
            string argumentValue = "Category";

            var exception = Record.Exception(() => DomainValidation.NotNull(argumentValue, "Value"));

            Assert.Null(exception);
        }

        [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("a",12)]
        [InlineData("Kia", 4)]
        [InlineData("Kuth", 6)]
        public void MinLengthThrowWhenLess(string target, int minLength)
        {
            Action action = () => DomainValidation.MinLength(target, minLength, "fieldName");

            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal($"fieldName should be less or equal {minLength} characters long", exception.Message);

        }

        [Theory(DisplayName = nameof(MinLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("Product with name ok", 5)]
        [InlineData("Valid target", 3)]
        [InlineData("Valid target ok", 4)]
        public void MinLengthOk(string target, int minLength)
        {
            var exception = Record.Exception(() => DomainValidation.MinLength(target,minLength ,"fieldName"));

            Assert.Null(exception);
        }

        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("Product with name error", 3)]
        [InlineData("Invalid target", 5)]
        [InlineData("Invalid target ok", 4)]
        public void MaxLengthThrowWhenGreater(string target, int maxLength) 
        {
            Action action = () => DomainValidation.MaxLength(target, maxLength, "fieldName");

            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal($"fieldName should be less or equal {maxLength} characters long", exception.Message);
        }


        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("Product with name ok", 200)]
        [InlineData("valid target", 500)]
        [InlineData("valid target ok", 130)]
        public void MaxLengthOk(string target, int maxLength)
        {
            var exception = Record.Exception(() => DomainValidation.MaxLength(target, maxLength, "fieldName"));

            Assert.Null(exception);
        }

    }
}
