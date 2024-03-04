using MediatR;
using MyFlix.Catalog.Application.Common;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Application.UseCases.CastMember.ListCastMember
{
	public class ListCastMembersInput : PaginatedListInput, IRequest<ListCastMembersOutput>
	{
		public ListCastMembersInput(int page, int perPage, string search, string sort, SearchOrder dir) : base(page, perPage, search, sort, dir)
		{
		}
		public ListCastMembersInput(): base(1, 15, "", "", SearchOrder.Asc)
		{ }
	}
}
