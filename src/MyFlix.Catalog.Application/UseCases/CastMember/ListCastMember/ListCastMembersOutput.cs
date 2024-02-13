using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;

namespace MyFlix.Catalog.Application.UseCases.CastMember.ListCastMember
{
	public class ListCastMembersOutput : PaginatedListOutput<CastMemberModelOutput>
	{
		public ListCastMembersOutput(int page, int perPage, int total, IReadOnlyList<CastMemberModelOutput> items) 
			: base(page, perPage, total, items)
		{
		}
	}
}
