using MediatR;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;

namespace MyFlix.Catalog.Application.UseCases.CastMember.GetCastMember
{
	public interface IGetCastMember : IRequestHandler<GetCastMemberInput, CastMemberModelOutput>
	{

	}
}
