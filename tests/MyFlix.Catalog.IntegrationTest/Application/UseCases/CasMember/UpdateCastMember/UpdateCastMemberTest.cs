using UseCase = MyFlix.Catalog.Application.UseCases.CastMember.UpdateCastMember;
using MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.Common;
using Xunit;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.Infra.Data.EF;
using FluentAssertions;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.UpdateCastMember
{
	[Collection(nameof(CastMemberUseCasesBaseFixture))]
	public class UpdateCastMemberTest
	{
		private readonly CastMemberUseCasesBaseFixture _fixture;

		public UpdateCastMemberTest(CastMemberUseCasesBaseFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Update))]
		[Trait("Integration/Application", "UpdateCastMember - Use Cases")]
		public async Task Update()
		{
			var examples = _fixture.GetExampleCastMembersList(10);
			var example = examples[5];
			var arrangeDbContext = _fixture.CreateDbContext();
			await arrangeDbContext.AddRangeAsync(examples);
			await arrangeDbContext.SaveChangesAsync();
			var newName = _fixture.GetValidName();
			var newType = _fixture.GetRandomCastMemberType();
			var actDbContext = _fixture.CreateDbContext(true);
			var repository = new CastMemberRepository(actDbContext);
			var uow = new UnitOfWork(actDbContext);
			var useCase = new UseCase.UpdateCastMember(repository, uow);
			var input = new UseCase.UpdateCastMemberInput(example.Id, newName, newType);

			var output = await useCase.Handle(input, CancellationToken.None);

			output.Should().NotBeNull();
			output.Id.Should().Be(example.Id);
			output.Name.Should().Be(newName);
			output.Type.Should().Be(newType);
			var item = await _fixture.CreateDbContext(true).CastMembers.FindAsync(example.Id);
			item.Should().NotBeNull();
			item!.Name.Should().Be(newName);
			item.Type.Should().Be(newType);
		}

		[Fact(DisplayName = nameof(ThrowWhenNotFound))]
		[Trait("Integration/Application", "UpdateCastMember - Use Cases")]
		public async Task ThrowWhenNotFound()
		{
			var randomGuid = Guid.NewGuid();
			var newName = _fixture.GetValidName();
			var newType = _fixture.GetRandomCastMemberType();
			var actDbContext = _fixture.CreateDbContext(true);
			var repository = new CastMemberRepository(actDbContext);
			var uow = new UnitOfWork(actDbContext);
			var useCase = new UseCase.UpdateCastMember(repository, uow);
			var input = new UseCase.UpdateCastMemberInput(randomGuid, newName, newType);

			var action = async () => await useCase.Handle(input, CancellationToken.None);

			await action.Should().ThrowAsync<NotFoundException>().WithMessage($"CastMember '{randomGuid}' not found");
		}
	}
}
