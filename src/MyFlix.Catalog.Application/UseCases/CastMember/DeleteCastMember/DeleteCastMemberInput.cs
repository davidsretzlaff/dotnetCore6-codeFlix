using MediatR;

namespace MyFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember
{
	public class DeleteCastMemberInput : IRequest
	{
		public DeleteCastMemberInput(Guid id) => Id = id;

		public Guid Id { get; private set; }
    }
}
