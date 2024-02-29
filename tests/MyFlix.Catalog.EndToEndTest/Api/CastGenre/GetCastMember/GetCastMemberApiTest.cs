﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.EndToEndTest.Api.CastGenre.Common;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MyFlix.Catalog.EndToEndTest.Api.CastGenre.GetCastMember
{

	[Collection(nameof(CastMemberApiBaseFixture))]
	public class GetCastMemberApiTest
	{
		private readonly CastMemberApiBaseFixture _fixture;

		public GetCastMemberApiTest(CastMemberApiBaseFixture fixture)
			=> _fixture = fixture;

		[Fact(DisplayName = nameof(Get))]
		[Trait("EndToEnd/API", "CatMembers/Get - EndPoints")]
		public async Task Get()
		{
			var examples = _fixture.GetExampleCastMembersList(5);
			var example = examples[2];
			await _fixture.Persistence.InsertList(examples);

			var (response, output) =
				await _fixture.ApiClient.Get<ApiResponse<CastMemberModelOutput>>(
					$"castmembers/{example.Id.ToString()}"
				);

			response.Should().NotBeNull();
			response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
			output.Should().NotBeNull();
			output!.Data.Should().NotBeNull();
			output.Data.Id.Should().Be(example.Id);
			output.Data.Name.Should().Be(example.Name);
			output.Data.Type.Should().Be(example.Type);
		}
	}
}
