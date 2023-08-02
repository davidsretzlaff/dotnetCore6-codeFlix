using FluentValidation;

namespace CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
    {
        public UpdateCategoryInputValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
