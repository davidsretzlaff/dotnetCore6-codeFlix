﻿namespace MyFlix.Catalog.Domain.Entity
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
        }

        public void Activate() => IsActive = true;
    }
}