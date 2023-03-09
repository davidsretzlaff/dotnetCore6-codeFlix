using CodeFlix.Catalog.Domain.Exceptions;
using CodeFlix.Catalog.Domain.SeedWork;
using System.Data;
using System.Net.Http.Headers;

namespace CodeFlix.Catalog.Domain.Entity
{
    public class Category : AggregateRoot
    {
        public Category(string name, string description, bool isActive = true) : base()
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
            IsActive = isActive;
            Validate();
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }
      
        public void Activate()
        {
            IsActive = true;
            Validate();
        }

        public void Deactivate()
        {
            IsActive = false;
            Validate();
        }

        public void Update(string name, string? description = null) 
        {
            Name = name;
            Description = description ?? Description;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
            if (Name.Length < 3)
                throw new EntityValidationException($"{nameof(Name)} should be at least 3 characters");
            if (Name.Length > 255)
                throw new EntityValidationException($"{nameof(Name)} should be less or equal 255 characters");
            if (Description == null)
                throw new EntityValidationException($"{nameof(Description)} should not be empty or null");
            if (Description.Length > 10_000)
                throw new EntityValidationException($"{nameof(Description)} should be less or equal 10.000 characters long");
        }
    }
}