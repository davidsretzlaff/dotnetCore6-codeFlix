﻿    using MyFlix.Catalog.Application.UseCases.Category.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyFlix.Catalog.EndToEndTest.Extensions.DataTime;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.Category.GetCategory;

namespace MyFlix.Catalog.EndToEndTest.Api.Category.GetCategory
{

    [Collection(nameof(GetCategoryApiTestFixture))]
    public class GetCategoryApiTest : IDisposable
    {        
        private readonly GetCategoryApiTestFixture _fixture;

        public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task GetCategory()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var exampleCategory = exampleCategoriesList[10];

            var (response, output) = await _fixture.ApiClient.Get<ApiResponse<CategoryModelOutput>>(
                $"/categories/{exampleCategory.Id}"
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(exampleCategory.Name);
            output.Data.Description.Should().Be(exampleCategory.Description);
            output.Data.IsActive.Should().Be(exampleCategory.IsActive);
            output.Data.CreatedAt.TrimMillisseconds().Should().Be(
                exampleCategory.CreatedAt.TrimMillisseconds()
            );
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task ErrorWhenNotFound()
        {
            var exampleCategoriesList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoriesList);
            var randomGuid = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>(
                $"/categories/{randomGuid}"
            );

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not Found");
            output.Detail.Should().Be($"Category '{randomGuid}' not found.");
        }

        public void Dispose() => _fixture.CleanPersistence();
    }
}
