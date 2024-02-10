using MediatR;

namespace MyFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember
{
	public interface IDeleteCastMember : IRequestHandler<DeleteCastMemberInput>
	{
	}
}
