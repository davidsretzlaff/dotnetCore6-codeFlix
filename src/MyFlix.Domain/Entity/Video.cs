using MyFlix.Catalog.Domain.Enum;
using MyFlix.Catalog.Domain.Exceptions;
using MyFlix.Catalog.Domain.SeedWork;
using MyFlix.Catalog.Domain.Validation;
using MyFlix.Catalog.Domain.Validator;
using MyFlix.Catalog.Domain.ValueObject;

namespace MyFlix.Catalog.Domain.Entity
{
	public class Video : AggregateRoot
	{
		public string Title { get; private set; }
		public string Description { get; private set; }
		public int YearLaunched { get; private set; }
		public bool Opened { get; private set; }
		public bool Published { get; private set; }
		public int Duration { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public Rating Rating { get; private set; }
		public Image? Thumb { get; private set; }
		public Image? ThumbHalf { get; private set; }
		public Image? Banner { get; private set; }
		public Media? Media { get; private set; }
		public Media? Trailer { get; private set; }

		public Video(string title, string description, int yearLaunched, bool opened, bool published, int duration, Rating rating)
		{
			Title = title;
			Description = description;
			YearLaunched = yearLaunched;
			Opened = opened;
			Published = published;
			Duration = duration;
			CreatedAt = DateTime.Now;
			Rating = rating;
		}
		public void UpdateThumb(string path)
			=> Thumb = new Image(path);

		public void UpdateThumbHalf(string path)
			=> ThumbHalf = new Image(path);

		public void UpdateBanner(string path)
			=> Banner = new Image(path);

		public void UpdateMedia(string path)
			=> Media = new Media(path);

		public void UpdateTrailer(string path)
			=> Trailer = new Media(path);

		public void UpdateAsSentToEncode()
		{
			if (Media is null)
				throw new EntityValidationException("There is no Media");
			Media.UpdateAsSentToEncode();
		}

		public void UpdateAsEncoded(string validEncodedPath)
		{
			if (Media is null)
				throw new EntityValidationException("There is no Media");
			Media.UpdateAsEncoded(validEncodedPath);
		}

		public void Update(string title, string description, int yearLaunched, bool opened, bool published, int duration)
		{
			Title = title;
			Description = description;
			YearLaunched = yearLaunched;
			Opened = opened;
			Published = published;
			Duration = duration;
		}

		public void Validate(ValidationHandler handler)
			   => (new VideoValidator(this, handler)).Validate();

	}
}
