using MediatR;

namespace MyFlix.Catalog.Application.UseCases.CastMember.ListCastMember
{
	public interface IListCastMembers : IRequestHandler<ListCastMembersInput, ListCastMembersOutput>
	{
	}
}
