using FluentAssertions;
using Moq;
using MyFlix.Catalog.Domain.Repository;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
using UseCase = MyFlix.Catalog.Application.UseCases.CastMember.ListCastMember;
namespace MyFlix.Catalog.UnitTests.Application.CastMember.ListCastMember
{
	[Collection(nameof(ListCastMemberTestFixture))]
	public class ListCastMemberTest 
	{
		private readonly ListCastMemberTestFixture _fixture;

		public ListCastMemberTest(ListCastMemberTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(List))]
		[Trait("Application", "ListCastMember - Use Cases")]
		public async Task List()
		{
			var repositoryMock = new Mock<ICastMemberRepository>();
			var castMemberListExample = _fixture.GetExampleCastMemberList(3);
			var repositorySearchOutput = new SearchOutput<DomainEntity.CastMember>(1, 10, castMemberListExample.Count(), castMemberListExample);
			repositoryMock.Setup(x => x.Search(
				It.IsAny<SearchInput>(), It.IsAny<CancellationToken>()
			)).ReturnsAsync(repositorySearchOutput);
			var input = new UseCase.ListCastMembersInput(1, 10,"","",SearchOrder.Asc);
			var useCase = new UseCase.ListCastMembers(repositoryMock.Object);

			var output = await useCase.Handle(input, CancellationToken.None);

			output.Should().NotBeNull();
			output.Page.Should().Be(repositorySearchOutput.CurrentPage);
			output.PerPage.Should().Be(repositorySearchOutput.PerPage);
			output.Total.Should().Be(repositorySearchOutput.Total);
			output.Items.ToList().ForEach(outputItem =>
			{
				var example = castMemberListExample.Find(x => x.Id == outputItem.Id);
				example.Should().NotBeNull();
				outputItem.Name.Should().Be(example.Name);
				outputItem.Type.Should().Be(example.Type);
			});
			repositoryMock.Verify(x => x.Search(
				It.Is<SearchInput>(x => (
					x.Page == input.Page &&
					x.PerPage == input.PerPage &&
					x.Search == input.Search &&
					x.Order == input.Dir &&
					x.OrderBy ==input.Sort
				)), It.IsAny<CancellationToken>()
			));
		}
	}
}
