using MyFlix.Catalog.Application.UseCases.Category.Common;
using MyFlix.Catalog.Application.UseCases.Category.CreateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System;

namespace MyFlix.Catalog.EndToEndTest.Api.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTest : IDisposable
    {
        private readonly CreateCategoryApiTestFixture _fixture;

        public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("EndToEnd/API", "Category/Create - Endpoints")]
        public async Task CreateCategory()
        {
            {
                var input = _fixture.getExampleInput();

                var (response, output) = await _fixture.
                    ApiClient.Post<CategoryModelOutput>(
                        "/categories",
                        input
                    );

                response.Should().NotBeNull();
                response!.StatusCode.Should().Be(HttpStatusCode.Created);
                output.Should().NotBeNull();
                output!.Name.Should().Be(input.Name);
                output.Description.Should().Be(input.Description);
                output.IsActive.Should().Be(input.IsActive);
                output.Id.Should().NotBeEmpty();
                output.CreatedAt.Should().NotBeSameDateAs(default);

                var dbCategory = await _fixture.Persistence.GetById(output.Id);
                dbCategory.Should().NotBeNull();
                dbCategory!.Name.Should().Be(input.Name);
                dbCategory.Description.Should().Be(input.Description);
            }
        }

        [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
        [Trait("EndToEnd/API", "Category/Create - Endpoints")]
        [MemberData(
            nameof(CreateCategoryApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(CreateCategoryApiTestDataGenerator)
        )]
        public async Task ErrorWhenCantInstantiateAggregate(CreateCategoryInput input, string expectedDetail)
        {
            var (response, output) = await _fixture.
                ApiClient.Post<ProblemDetails>(
                    "/categories",
                    input
                );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output!.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be(expectedDetail);
        }

        public void Dispose() => _fixture.CleanPersistence();
    }
}
