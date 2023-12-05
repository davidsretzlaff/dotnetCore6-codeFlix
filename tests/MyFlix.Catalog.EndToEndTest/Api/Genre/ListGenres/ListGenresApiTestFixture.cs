using MyFlix.Catalog.EndToEndTest.Api.Genre.Common;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using DomainEntity = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.EndToEndTest.Api.Genre.ListGenres
{

    [CollectionDefinition(nameof(ListGenresApiTestFixture))]
    public class ListGenresApiTestFixtureCiollection : ICollectionFixture<ListGenresApiTestFixture> 
    {
    }

    public class ListGenresApiTestFixture : GenreBaseFixture
    {
        public List<DomainEntity.Genre> GetExampleListGenresByNames(List<string> names)
        {
            return names.Select(name => GetExampleGenre(name: name)).ToList();
        }
	}
}
