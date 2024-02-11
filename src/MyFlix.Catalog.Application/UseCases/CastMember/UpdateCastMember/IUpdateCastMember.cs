using MediatR;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;

namespace MyFlix.Catalog.Application.UseCases.CastMember.UpdateCastMember
{
	public interface IUpdateCastMember : IRequestHandler<UpdateCastMemberInput, CastMemberModelOutput>
	{

	}
}
