using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategoryInputValidator : AbstractValidator<DeleteCategoryInput>
    {
        public DeleteCategoryInputValidator()
           => RuleFor(x => x.Id).NotEmpty();
    }
}
