using MediatR;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;

namespace MyFlix.Catalog.Application.UseCases.CastMember.CreateCastMember
{
	public interface ICreateCastMember : IRequestHandler<CreateCastMemberInput, CastMemberModelOutput>
	{

	}
}
