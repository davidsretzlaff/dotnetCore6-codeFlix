
using MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Domain.Validation
{
	public class VideoValidator : Validation.Validator
	{
		private readonly Video _video;

		public VideoValidator(Video video, ValidationHandler handler) : base(handler)
				=> _video = video;

		public override void Validate()
		{
		}
	}
}
