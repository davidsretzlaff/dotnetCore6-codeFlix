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

		private List<Guid> _categories;
		public IReadOnlyList<Guid> Categories => _categories.AsReadOnly();

		private List<Guid> _genres;
		public IReadOnlyList<Guid> Genres => _genres.AsReadOnly();

		private List<Guid> _castMembers;
		public IReadOnlyList<Guid> CastMembers => _castMembers.AsReadOnly();

		public Video(string title, string description, int yearLaunched, bool opened, bool published, int duration, Rating rating)
		{
			Title = title;
			Description = description;
			YearLaunched = yearLaunched;
			Opened = opened;
			Published = published;
			Duration = duration;
			_categories = new();
			CreatedAt = DateTime.Now;
			Rating = rating;
			_genres = new();
			_castMembers = new();
		}

		public void AddCategory(Guid categoryId)
			=> _categories.Add(categoryId);

		public void Removecategory(Guid categoryId)
			=> _categories.Remove(categoryId);

		public void RemoveAllCategory()
			=> _categories = new();

		public void AddGenre(Guid genreId)
			=> _genres.Add(genreId);

		public void RemoveGenre(Guid genreId)
			=> _genres.Remove(genreId);

		public void RemoveAllGenres()
			=> _genres = new();

		public void AddCastMember(Guid castMemberId)
			=> _castMembers.Add(castMemberId);

		public void RemoveCastMember(Guid castMemberid)
			=> _castMembers.Remove(castMemberid);

		public void RemoveAllCastMembers()
			=> _castMembers = new();

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

		public void Update(
		   string title,
		   string description,
		   int yearLaunched,
		   bool opened,
		   bool published,
		   int duration,
		   Rating? rating = null)
		{
			Title = title;
			Description = description;
			YearLaunched = yearLaunched;
			Opened = opened;
			Published = published;
			Duration = duration;
			if (rating is not null)
				Rating = (Rating)rating;
		}

		public void Validate(ValidationHandler handler)
			   => (new VideoValidator(this, handler)).Validate();

	}
}
