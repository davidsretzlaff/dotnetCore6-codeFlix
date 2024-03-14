
using MyFlix.Catalog.Domain.Validation;

namespace MyFlix.Catalog.Domain.Exceptions
{
    public class EntityValidationException : Exception
	{
		public IReadOnlyCollection<ValidationError>? Errors { get; }
		public EntityValidationException(
			string? message,
			IReadOnlyCollection<ValidationError>? errors = null
		) : base(message)
		{ 
			Errors = errors;
		}
    }
}
