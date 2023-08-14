using FluentAssertions;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.UnitTests.Domain.Entity.Genre
{
    [Collection(nameof(GenreTestFixture))]
    public class GenreTest
    {

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Genre - Aggregates")]
        public void Instantiate()
        {
            var genreName = "Horror";

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
    }
}
