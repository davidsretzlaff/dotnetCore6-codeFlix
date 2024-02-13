using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.Application.UseCases.CastMember.ListCastMember
{
	public class ListCastMembersOutput : PaginatedListOutput<CastMemberModelOutput>
	{
		public ListCastMembersOutput(int page, int perPage, int total, IReadOnlyList<CastMemberModelOutput> items) 
			: base(page, perPage, total, items)
		{
		}
		
		public static ListCastMembersOutput FromSearchOutput(SearchOutput<DomainEntity.CastMember> searchOutput)
		{
			return new ListCastMembersOutput(searchOutput.CurrentPage,
				searchOutput.PerPage,
				searchOutput.Total,
				searchOutput.Items.Select(
						castMember => CastMemberModelOutput.FromCastMember(castMember)).ToList()
			);
		}
	}
}
