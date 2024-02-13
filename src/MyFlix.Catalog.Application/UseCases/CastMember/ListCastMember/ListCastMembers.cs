

using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Application.UseCases.CastMember.ListCastMember
{
	public class ListCastMembers : IListCastMembers
	{
		private readonly ICastMemberRepository _repository;

		public ListCastMembers(ICastMemberRepository repository) => _repository = repository;

		public async Task<ListCastMembersOutput> Handle(ListCastMembersInput request, CancellationToken cancellationToken)
		{
			var searchOutput = await _repository.Search(request.ToSearchInput(), cancellationToken);
			return ListCastMembersOutput.FromSearchOutput(searchOutput);
		}
	}
}
