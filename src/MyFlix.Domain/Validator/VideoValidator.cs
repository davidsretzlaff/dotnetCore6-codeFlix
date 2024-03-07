using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Validation;

namespace MyFlix.Catalog.Domain.Validator
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
