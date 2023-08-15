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
            Name = name;
            IsActive = isActive;
            CreatedAt = DateTime.Now;
            Validate();
        }

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
        public void Update(string name)
        {
            Name = name;
            Validate();
        }
        public void Validate()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        }
    }
}
