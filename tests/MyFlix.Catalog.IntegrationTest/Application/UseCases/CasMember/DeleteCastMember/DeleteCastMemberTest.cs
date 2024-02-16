using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Infra.Data.EF;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.Common;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.DeleteCastMember
{
	[Collection(nameof(CastMemberUseCasesBaseFixture))]
	public class DeleteCastMemberTest
	{
		private readonly CastMemberUseCasesBaseFixture _fixture;

		public DeleteCastMemberTest(CastMemberUseCasesBaseFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Delete))]
		[Trait("Integration/Application", "DeleteCastMember - Use Cases")]
		public async Task Delete()
		{
			var example = _fixture.GetExampleCastMember();
			var arrangeDbContext = _fixture.CreateDbContext();
			await arrangeDbContext.AddAsync(example);
			await arrangeDbContext.SaveChangesAsync();
			var actDbContext = _fixture.CreateDbContext(true);
			var repository = new CastMemberRepository(actDbContext);
			var unitOfWork = new UnitOfWork(actDbContext);
			var useCase = new UseCase.DeleteCastMember(repository, unitOfWork);
			var input = new UseCase.DeleteCastMemberInput(example.Id);

			await useCase.Handle(input, CancellationToken.None);

			var assertDbContext = _fixture.CreateDbContext(true);
			var list = await assertDbContext.CastMembers.AsNoTracking().ToListAsync();
			list.Should().HaveCount(0);
		}

		[Fact(DisplayName = nameof(ThrowWhenNotFound))]
		[Trait("Integration/Application", "DeleteCastMember - Use Cases")]
		public async Task ThrowWhenNotFound()
		{
			var actDbContext = _fixture.CreateDbContext(true);
			var repository = new CastMemberRepository(actDbContext);
			var unitOfWork = new UnitOfWork(actDbContext);
			var useCase = new UseCase.DeleteCastMember(repository, unitOfWork);
			var randomGuid = Guid.NewGuid();
			var input = new UseCase.DeleteCastMemberInput(randomGuid);

			var action = async () => await useCase.Handle(input, CancellationToken.None);

			await action.Should().ThrowAsync<NotFoundException>().WithMessage($"CastMember '{randomGuid}' not found");
		}
	}
}
