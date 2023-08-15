using MyFlix.Catalog.Domain.Validation;

namespace MyFlix.Catalog.Domain.Entity
{
    public class Genre
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Genre(string name, bool isActive = true)
        {
            Validate();
            Name = name;
            IsActive = isActive;
            CreatedAt = DateTime.Now;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
        public void Update(string name) => Name = name;

        public void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        }
    }
}
