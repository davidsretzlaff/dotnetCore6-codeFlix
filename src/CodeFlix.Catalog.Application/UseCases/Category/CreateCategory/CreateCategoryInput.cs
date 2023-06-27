using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategoryInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public CreateCategoryInput(
            string name, 
            string? description = null, 
            bool isActive = true
            )
        {
            Name = name;
            Description = description ?? string.Empty;
            IsActive = isActive;
        }
    }
}
