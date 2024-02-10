using MediatR;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.Domain.Enum;

namespace MyFlix.Catalog.Application.UseCases.CastMember.CreateCastMember
{
	public class CreateCastMemberInput : IRequest<CastMemberModelOutput>
	{	
		public string Name { get; private set; }
		public CastMemberType Type { get; private set; }

		public CreateCastMemberInput(string name, CastMemberType type)
		{
			Name = name;
			Type = type;
		}
	}
}
