
namespace CodeFlix.Catalog.Domain.Exceptions
{
    public class EntityValidationException : Exception
    {
        public EntityValidationException(string? message) : base(message)
        {
            throw new ($"{fieldName} should be less or equal {maxLength} characters long");
        }
    }
}
