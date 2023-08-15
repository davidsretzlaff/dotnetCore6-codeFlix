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
            var datetimeBefore = DateTime.Now;
            var genre = _fixture.GetExampleGenre();
            var datetimeAfter = DateTime.Now.AddSeconds(3);

            genre.Should().NotBeNull();
            genre.Name.Should().Be(genre.Name);
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
            var datetimeBefore = DateTime.Now;
            var genre = _fixture.GetExampleGenre(isActive);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            genre.Should().NotBeNull();
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
            var genre = _fixture.GetExampleGenre(isActive);
            var oldName = genre.Name;
            genre.Activate();

            genre.Should().NotBeNull();
            genre.Name.Should().Be(oldName);
            genre.IsActive.Should().BeTrue();
            genre.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(Deactivate))]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Domain", "Genre - Aggregates")]
        public void Deactivate(bool isActive)
        {
            var genre = _fixture.GetExampleGenre(isActive);
            var oldName = genre.Name;

            genre.Deactivate();

            genre.Should().NotBeNull();
            genre.Name.Should().Be(oldName);
            genre.IsActive.Should().BeFalse();
            genre.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = (nameof(Update)))]
        [Trait("Domain", "Genre - Aggregates")]
        public void Update()
        {
            var genre = _fixture.GetExampleGenre();
            var newName = _fixture.GetValidName();
            var oldIsActive = genre.IsActive;

            genre.Update(newName);

            genre.Should().NotBeNull();
            genre.Name.Should().Be(newName);
            genre.IsActive.Should().Be(oldIsActive);
            genre.CreatedAt.Should().NotBeSameDateAs(default);
        }
    }
}
