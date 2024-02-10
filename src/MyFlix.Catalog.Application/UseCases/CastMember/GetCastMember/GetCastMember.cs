using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.Domain.Repository;

namespace MyFlix.Catalog.Application.UseCases.CastMember.GetCastMember
{
	public class GetCastMember : IGetCastMember
	{
		private readonly ICastMemberRepository _repository;

		public GetCastMember(ICastMemberRepository repository) => _repository = repository;

		public async Task<CastMemberModelOutput> Handle(GetCastMemberInput request, CancellationToken cancellationToken)
		{
			var castMember = await _repository.Get(request.Id, cancellationToken);
			return CastMemberModelOutput.FromCastMember(castMember);
		}
	}
}
