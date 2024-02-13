using Xunit;

namespace MyFlix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CastMemberRepository
{
	[Collection(nameof(CastMemberRepositoryTestFixture))]
	public class CastMemberRepositoryTest
	{
		private readonly CastMemberRepositoryTestFixture _fixture;

		public CastMemberRepositoryTest(CastMemberRepositoryTestFixture fixture) => _fixture = fixture;

		[Fact(DisplayName =nameof(Insert))]
		[Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
		public async Task Insert()
		{
			var castMemberExample = _fixture.GetExampleCastMember();
			var context = _fixture.CreateDbContext();
			var repository = new CastMemberRepository(context);

			await repository.Insert(castMemberExample, CancellationToken.None);
			context .SaveChanges();

			// persist state
			var assertionContext = _fixture.CreateDbContext(true);
			var castMemberFromDb = assertionContext.SaveChanges().AsNoTracking().FirstOrDefaultAsync(x => x.Id == castMemberExample.Id);
			castMemberFromDb.Name.Should().Be(castMemberExample.Name);
			castMemberFromDb.Type.Should().Be(castMemberExample.Type);
		}
	}
}
