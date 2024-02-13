

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
			var searchOutput = await _repository.Search(
				new SearchInput(request.Page,request.PerPage,request.Search,request.Sort,request.Dir),
				cancellationToken);
			return new ListCastMembersOutput(
				searchOutput.CurrentPage,
				searchOutput.PerPage,
				searchOutput.Total,
				searchOutput.Items.Select(
						castMember => CastMemberModelOutput.FromCastMember(castMember)).ToList()
					);
		}
	}
}
