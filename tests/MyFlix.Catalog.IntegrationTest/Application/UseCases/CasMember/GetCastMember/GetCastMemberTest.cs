using FluentAssertions;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.Common;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.CastMember.GetCastMember;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.GetCastMember
{
	[Collection(nameof(CastMemberUseCasesBaseFixture))]
	public class GetCastMemberTest
	{
		private readonly CastMemberUseCasesBaseFixture _fixture;

		public GetCastMemberTest(CastMemberUseCasesBaseFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(GetCastMember))]
		[Trait("Integration/Application", "GetCastMember - Use Cases")]
		public async Task GetCastMember()
		{
			var examples = _fixture.GetExampleCastMembersList(10);
			var exampleCastMember = examples[5];
			var arrangeDbContest = _fixture.CreateDbContext();
			await arrangeDbContest.AddRangeAsync(examples);
			await arrangeDbContest.SaveChangesAsync();
			var useCase = new UseCase.GetCastMember(
				new CastMemberRepository(_fixture.CreateDbContext(true)
			));
			var input = new UseCase.GetCastMemberInput(exampleCastMember.Id);

			var output = await useCase.Handle(input, CancellationToken.None);
			
			output.Should().NotBeNull();
			output.Name.Should().Be(exampleCastMember.Name);
			output.Type.Should().Be(exampleCastMember.Type);
			output.Id.Should().Be(exampleCastMember.Id);
		}

		[Fact(DisplayName = nameof(ThrowWhenNotFound))]
		[Trait("Integration/Application", "GetCastMember - Use Cases")]
		public async Task ThrowWhenNotFound()
		{
			var useCase = new UseCase.GetCastMember(
				new CastMemberRepository(_fixture.CreateDbContext())
			);
			var randomGuid = Guid.NewGuid();
			var input = new UseCase.GetCastMemberInput(randomGuid);

			var action = async () => await useCase.Handle(input, CancellationToken.None);

			await action.Should().ThrowAsync<NotFoundException>().WithMessage($"CastMember '{randomGuid}' not found");
		}
	}
}
