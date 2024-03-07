﻿using MyFlix.Catalog.Domain.Entity;
using MyFlix.Catalog.Domain.Validation;

namespace MyFlix.Catalog.Domain.Validator
{
    public class VideoValidator : Validation.Validator
    {
        private readonly Video _video;
		private const int TitleMaxLength = 255;

		public VideoValidator(Video video, ValidationHandler handler) : base(handler)
                => _video = video;

        public override void Validate()
        {
			if (_video.Title.Length > 255)
				_handler.HandleError($"'{nameof(_video.Title)}' should be less or equal {TitleMaxLength} characters long");
		}
    }
}
