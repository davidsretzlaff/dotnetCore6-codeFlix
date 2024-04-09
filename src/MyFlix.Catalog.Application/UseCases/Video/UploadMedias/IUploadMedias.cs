using MediatR;
namespace MyFlix.Catalog.Application.UseCases.Video.UploadMedias
{
	public interface IUploadMedias : IRequestHandler<UploadMediasInput>
	{ }
}
