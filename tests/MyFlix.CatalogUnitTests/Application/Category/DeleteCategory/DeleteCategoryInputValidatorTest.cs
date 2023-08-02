using CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory;
using CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodeFlix.Catalog.UnitTests.Application.Category.DeleteCategory
{

    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryInputValidatorTest
    {
        public DeleteCategoryInputValidatorTest(DeleteCategoryTestFixture fixture) => _fixture = fixture;
        private readonly DeleteCategoryTestFixture _fixture;

        [Fact(DisplayName = "")]
        [Trait("Application", "DeleteCategoryInputValidation - UseCases")]
        public void ValidationOk()
        {
            var validInput = new DeleteCategoryInput(Guid.NewGuid());
            var validator = new DeleteCategoryInputValidator();

            var result = validator.Validate(validInput);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "")]
        [Trait("Application", "InvalidWhenEmptyId - UseCases")]
        public void InvalidWhenEmptyGuidId()
        {
            var invalidInput = new DeleteCategoryInput(Guid.Empty);
            var validator = new DeleteCategoryInputValidator();

            var result = validator.Validate(invalidInput);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }
    }
}
