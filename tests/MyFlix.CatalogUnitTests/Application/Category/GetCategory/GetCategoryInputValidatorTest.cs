using MyFlix.Catalog.Application.UseCases.Category.GetCategory;
using Xunit;
using FluentAssertions;

namespace MyFlix.Catalog.UnitTests.Application.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryInputValidatorTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryInputValidatorTest(GetCategoryTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "")]
        [Trait("Application", "GetCategoryInputValidation - UseCases")]
        public void ValidationOk()
        {
            var validInput = new GetCategoryInput(Guid.NewGuid());
            var validator = new GetCategoryInputValidator();

            var result = validator.Validate(validInput);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Errors.Should().HaveCount(0);
        }


        [Fact(DisplayName = "")]
        [Trait("Application", "InvalidWhenEmptyId - UseCases")]
        public void InvalidWhenEmptyGuidId()
        {
            var invalidInput = new GetCategoryInput(Guid.Empty);
            var validator = new GetCategoryInputValidator();

            var result = validator.Validate(invalidInput);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
        }
    }
}
