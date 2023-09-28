using MyFlix.Catalog.EndToEndTest.Base;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.Common
{
    public class GenreBaseFixture : BaseFixture
    {
        public GenrePersistence Persistence { get; set; }

        public GenreBaseFixture() : base()
        {
            Persistence = new GenrePersistence(CreateDbContext());
        }
    }
}
