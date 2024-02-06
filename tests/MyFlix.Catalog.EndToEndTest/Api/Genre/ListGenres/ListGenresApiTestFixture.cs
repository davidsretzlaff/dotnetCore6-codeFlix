﻿using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;
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

		public List<DomainEntity.Genre> CloneGenreListOrdered(List<DomainEntity.Genre> genreList, string orderBy, SearchOrder order)
		{
			var listClone = new List<DomainEntity.Genre>(genreList);
			var orderedEnumerable = (orderBy.ToLower(), order) switch
			{
				("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
				("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
				("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
				("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
				("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
				("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
				_ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
			};
			return orderedEnumerable.ToList();
		}
	}
}
