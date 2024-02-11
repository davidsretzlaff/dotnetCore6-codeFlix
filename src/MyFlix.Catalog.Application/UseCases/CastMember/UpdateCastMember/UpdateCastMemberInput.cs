using MediatR;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.Domain.Enum;

namespace MyFlix.Catalog.Application.UseCases.CastMember.UpdateCastMember
{
	public class UpdateCastMemberInput : IRequest<CastMemberModelOutput>
	{
		public UpdateCastMemberInput(Guid id, string name, CastMemberType type)
		{
			Id = id;
			Name = name;
			Type = type;
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public CastMemberType Type { get; set; }

	}
}
