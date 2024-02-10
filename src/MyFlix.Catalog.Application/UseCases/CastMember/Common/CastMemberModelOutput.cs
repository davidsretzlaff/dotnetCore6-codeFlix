using MyFlix.Catalog.Domain.Enum;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Application.UseCases.CastMember.Common
{
	public class CastMemberModelOutput
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public CastMemberType Type { get; set; }
		public DateTime CreatedAt { get; set; }


		public CastMemberModelOutput(Guid id, string name, CastMemberType type, DateTime createdAt)
		{
			Id = id;
			Name = name;
			Type = type;
			CreatedAt = createdAt;
		}

		public static CastMemberModelOutput FromCastMember(DomainEntity.CastMember castMember)
			=> new CastMemberModelOutput(
				castMember.Id,
				castMember.Name,
				castMember.Type,
				castMember.CreatedAt
			);
		
	}
}
