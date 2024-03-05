﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFlix.Catalog.Api.ApiModels.CastMember;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.CastMember.Common;
using MyFlix.Catalog.Application.UseCases.CastMember.CreateCastMember;
using MyFlix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
using MyFlix.Catalog.Application.UseCases.CastMember.GetCastMember;
using MyFlix.Catalog.Application.UseCases.CastMember.ListCastMember;
using MyFlix.Catalog.Application.UseCases.CastMember.UpdateCastMember;
using MyFlix.Catalog.Domain.SeedWork.SearchableRepository;

namespace MyFlix.Catalog.Api.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class CastMembersController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CastMembersController(IMediator mediator) => _mediator = mediator;

		[HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(ApiResponse<CastMemberModelOutput>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			var output = await _mediator.Send(new GetCastMemberInput(id), cancellationToken);
			return Ok(new ApiResponse<CastMemberModelOutput>(output));
		}

		[HttpDelete("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
		{
			await _mediator.Send(new DeleteCastMemberInput(id), cancellationToken);
			return NoContent();
		}

		[HttpPost]
		[ProducesResponseType(typeof(ApiResponse<CastMemberModelOutput>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Create([FromBody] CreateCastMemberInput input, CancellationToken cancellationToken)
		{
			var output = await _mediator.Send(input, cancellationToken);
			return CreatedAtAction(
				nameof(GetById),
				new { Id = output.Id },
				new ApiResponse<CastMemberModelOutput>(output)
			);
		}

		[HttpPut("{id:guid}")]
		[ProducesResponseType(typeof(ApiResponse<CastMemberModelOutput>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCastMemberApiInput apiInput, CancellationToken cancellationToken)
		{
			var output = await _mediator.Send(
				new UpdateCastMemberInput(id, apiInput.Name, apiInput.Type),
				cancellationToken
			);
			return Ok(new ApiResponse<CastMemberModelOutput>(output));
		}

		[HttpGet]
		[ProducesResponseType(typeof(ApiResponseList<CastMemberModelOutput>), StatusCodes.Status200OK)]
		public async Task<IActionResult> List(
			[FromQuery] int? page,
			[FromQuery(Name = "per_page")] int? perPage,
			[FromQuery] string? search,
			[FromQuery] SearchOrder? dir,
			[FromQuery] string? sort,
			CancellationToken cancellationToken)
			{ 
			var input = new ListCastMembersInput();
			if (page is not null) input.Page = page.Value;
			if (perPage is not null) input.PerPage = perPage.Value;
			if (search is not null) input.Search = search;
			if (dir is not null) input.Dir = dir.Value;
			if (sort is not null) input.Sort = sort;
			var output = await _mediator.Send(input, cancellationToken);
			return Ok(new ApiResponseList<CastMemberModelOutput>(output));
		}
	}
}
