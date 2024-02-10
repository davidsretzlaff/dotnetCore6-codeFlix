using MediatR;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;

namespace MyFlix.Catalog.Application.UseCases.CastMember.GetCastMember
{
	public class GetCastMemberInput : IRequest<CastMemberModelOutput>
	{
		public GetCastMemberInput(Guid id) => Id = id;

		public Guid Id { get; private set; }
    }
}
