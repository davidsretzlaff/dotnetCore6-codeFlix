
using MyFlix.Catalog.Domain.Enum;
using MyFlix.Catalog.Domain.SeedWork;
using MyFlix.Catalog.Domain.Validation;

namespace MyFlix.Catalog.Domain.Entity
{
	public class CastMember : AggregateRoot
	{
		public string Name { get; private set; }
		public CastMemberType Type { get; private set; }
		public DateTime CreatedAt { get; private set; }

		private void Validate() => DomainValidation.NotNullOrEmpty(Name, nameof(Name));

		public CastMember(string name, CastMemberType type)
		: base()
		{
			Name = name;
			Type = type;
			CreatedAt = DateTime.Now;
			Validate();
		}

	}
}
