
using MediatR;
using MyFlix.Catalog.Application.UseCases.Video.Common;

namespace MyFlix.Catalog.Application.UseCases.Video.UploadMedias
{
	public record UploadMediasInput(
		Guid VideoId,
		FileInput? VideoFile,
		FileInput? TrailerFile) : IRequest;
}
