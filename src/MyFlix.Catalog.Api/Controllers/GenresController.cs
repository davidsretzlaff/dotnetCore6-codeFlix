﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFlix.Catalog.Api.ApiModels.Genre;
using MyFlix.Catalog.Api.ApiModels.Response;
using MyFlix.Catalog.Application.UseCases.Genre.Common;
using MyFlix.Catalog.Application.UseCases.Genre.CreateGenre;
using MyFlix.Catalog.Application.UseCases.Genre.DeleteGenre;
using MyFlix.Catalog.Application.UseCases.Genre.GetGenre;
using MyFlix.Catalog.Application.UseCases.Genre.UpdateGenre;

namespace MyFlix.Catalog.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenresController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<GenreModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(new GetGenreInput(id), cancellationToken);
            return Ok(new ApiResponse<GenreModelOutput>(output));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteGenreInput(id), cancellationToken);
            return NoContent();
        }

        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse<GenreModelOutput>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreInput input, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(input, cancellationToken);
            return CreatedAtAction(
                nameof(GetById),
                new { id = output.Id },
                new ApiResponse<GenreModelOutput>(output)
            );
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<GenreModelOutput>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGenre(
        [FromBody] UpdateGenreApiInput apiInput, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var output = await _mediator.Send(
                new UpdateGenreInput(
                    id,
                    apiInput.Name,
                    apiInput.IsActive,
                    apiInput.CategoriesIds
                ),
                cancellationToken
            );
            return Ok(new ApiResponse<GenreModelOutput>(output));
        }
    }
}
