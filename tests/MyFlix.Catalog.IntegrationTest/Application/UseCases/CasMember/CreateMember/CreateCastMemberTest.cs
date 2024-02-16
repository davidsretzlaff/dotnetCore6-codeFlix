using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Application.UseCases.CastMember.ListCastMember;
using MyFlix.Catalog.Infra.Data.EF;
using MyFlix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using UseCase = MyFlix.Catalog.Application.UseCases.CastMember.CreateCastMember;

namespace MyFlix.Catalog.IntegrationTest.Application.UseCases.CasMember.CreateMember
{

	[Collection(nameof(CreateCastMemberTestFixture))]
	public class CreateCastMemberTest
	{
		private readonly CreateCastMemberTestFixture _fixture;

		public CreateCastMemberTest(CreateCastMemberTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName = nameof(CreateCastMember))]
		[Trait("Integration/Application", "CreateCastMember - Use Cases")]
		public async Task CreateCastMember()
		{
			var actDbContext = _fixture.CreateDbContext();
			var repository = new CastMemberRepository(actDbContext);
			var unitOfWork = new UnitOfWork(actDbContext);
			var useCase = new UseCase.CreateCastMember(repository, unitOfWork);
			var input = new UseCase.CreateCastMemberInput(_fixture.GetValidName(), _fixture.GetRandomCastMemberType());

			var output = await useCase.Handle(input, CancellationToken.None);

			output.Should().NotBeNull();
			output.Name.Should().Be(input.Name);
			output.Type.Should().Be(input.Type);
			output.Id.Should().NotBeEmpty();
			output.CreatedAt.Should().NotBe(default(DateTime));
			var assertDbContext = _fixture.CreateDbContext(true);
			var castMembrers = await assertDbContext.CastMembers.AsNoTracking().ToListAsync();
			castMembrers.Should().HaveCount(1);
			var castMemberFromDb = castMembrers[0];
			castMemberFromDb.Name.Should().Be(input.Name);
			castMemberFromDb.Type.Should().Be(input.Type);
			castMemberFromDb.Id.Should().Be(output.Id);
		}
	}
}
