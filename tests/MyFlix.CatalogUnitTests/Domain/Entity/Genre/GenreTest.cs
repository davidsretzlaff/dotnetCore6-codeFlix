using FluentAssertions;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.UnitTests.Domain.Entity.Genre
{
    [Collection(nameof(GenreTestFixture))]
    public class GenreTest
    {
        private readonly GenreTestFixture _fixture;

        public GenreTest(GenreTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Genre - Aggregates")]
        public void Instantiate()
        {
            var genreName = _fixture.GetValidName();

            var datetimeBefore = DateTime.Now;
            var genre = new DomainEntity.Genre(genreName);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            genre.Should().NotBeNull();
            genre.Name.Should().Be(genreName);
            genre.IsActive.Should().BeTrue();
            genre.CreatedAt.Should().NotBeSameDateAs(default);
            (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
            (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Domain", "Genre - Aggregates")]
        public void InstantiateWithIsActive(bool isActive)
        {
            var genreName = _fixture.GetValidName();

            var datetimeBefore = DateTime.Now;
            var genre = new DomainEntity.Genre(genreName, isActive);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            genre.Should().NotBeNull();
            genre.Name.Should().Be(genreName);
            genre.IsActive.Should().Be(isActive);
            genre.CreatedAt.Should().NotBeSameDateAs(default);
            (genre.CreatedAt >= datetimeBefore).Should().BeTrue();
            (genre.CreatedAt <= datetimeAfter).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(Activate))]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Domain", "Genre - Aggregates")]
        public void Activate(bool isActive)
        {
            var genreName = _fixture.GetValidName();
            var genre = new DomainEntity.Genre(genreName, isActive);

            genre.Activate();

            genre.Should().NotBeNull();
            genre.Name.Should().Be(genreName);
            genre.IsActive.Should().BeTrue();
            genre.CreatedAt.Should().NotBeSameDateAs(default);
        }
    }
}
