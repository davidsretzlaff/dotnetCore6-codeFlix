﻿using MyFlix.Catalog.UnitTests.Common;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Domain.Entity.Genre
{

    [CollectionDefinition(nameof(GenreTestFixture))]
    public class GenreTestFixtureCollection : ICollectionFixture<GenreTestFixture> { }

    public class GenreTestFixture : BaseFixture
    {
    }
}
