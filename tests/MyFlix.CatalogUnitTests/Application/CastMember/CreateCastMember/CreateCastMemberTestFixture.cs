using MyFlix.Catalog.UnitTests.Application.CastMember.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFlix.Catalog.UnitTests.Application.CastMember.CreateCastMember
{
	[CollectionDefinition(nameof(CreateCastMemberTestFixture))]
	public class CreateCastMemberTestFixtureCollection : ICollectionFixture<CreateCastMemberTestFixture>
	{ }

	public class CreateCastMemberTestFixture : CastMemberUseCasesBaseFixture
	{
	}
}
