﻿using MyFlix.Catalog.Application.Exceptions;
using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Domain.Exceptions;
using MyFlix.Catalog.Domain.Repository;
using DomainEntities = MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Application.UseCases.Video.CreateVideo
{
	public class CreateVideo : ICreateVideo
	{
		private readonly IVideoRepository _videoRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICategoryRepository _categoryRepository;
		private readonly ICastMemberRepository _castMemberRepository;
		private readonly IGenreRepository _genreRepository;

		public CreateVideo(IVideoRepository videoRepository, ICategoryRepository categoryRepository, IGenreRepository genreRepository, ICastMemberRepository castMemberRepository, IUnitOfWork unitOfWork)
		{
			_videoRepository = videoRepository;
			_categoryRepository = categoryRepository;
			_genreRepository = genreRepository;
			_castMemberRepository = castMemberRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<CreateVideoOutput> Handle(
			CreateVideoInput input,
			CancellationToken cancellationToken)
		{
			var video = new DomainEntities.Video(
				input.Title,
				input.Description,
				input.YearLaunched,
				input.Opened,
				input.Published,
				input.Duration,
				input.Rating);

			var validationHandler = new NotificationValidationHandler();
			video.Validate(validationHandler);
			if (validationHandler.HasErrors())
			{
				throw new EntityValidationException("There are validation errors", validationHandler.Errors);
			}

			if ((input.CategoriesIds?.Count ?? 0) > 0)
			{
				var persistenceIds = await _categoryRepository.GetIdsListByIds(input.CategoriesIds!.ToList(), cancellationToken);
				
				if (persistenceIds.Count < input.CategoriesIds!.Count)
				{
					var notFoundIds = input.CategoriesIds!.ToList()
						.FindAll(categoryId => !persistenceIds.Contains(categoryId));
					
					throw new RelatedAggregateException(
						$"Related category id (or ids) not found: {string.Join(',', notFoundIds)}.");
				}
				
				input.CategoriesIds!.ToList().ForEach(video.AddCategory);
			}

			if ((input.GenresIds?.Count ?? 0) > 0)
			{
				var persistenceIds = await _genreRepository.GetIdsListByIds(input.GenresIds!.ToList(), cancellationToken);
				if (persistenceIds.Count < input.GenresIds!.Count)
				{
					var notFoundIds = input.GenresIds!.ToList().FindAll(id => !persistenceIds.Contains(id));
					
					throw new RelatedAggregateException(
						$"Related genre id (or ids) not found: {string.Join(',', notFoundIds)}.");
				}
				input.GenresIds!.ToList().ForEach(video.AddGenre);
			}

			if ((input.CastMembersIds?.Count ?? 0) > 0)
			{
				var persistenceIds = await _castMemberRepository.GetIdsListByIds(input.CastMembersIds!.ToList(), cancellationToken);
				if (persistenceIds.Count < input.CastMembersIds!.Count)
				{
					var notFoundIds = input.CastMembersIds!.ToList().FindAll(id => !persistenceIds.Contains(id));
					throw new RelatedAggregateException(
						$"Related cast member id (or ids) not found: {string.Join(',', notFoundIds)}.");
				
				}
				input.CastMembersIds!.ToList().ForEach(video.AddCastMember);
			}

			await _videoRepository.Insert(video, cancellationToken);
			await _unitOfWork.Commit(cancellationToken);

			return CreateVideoOutput.FromVideo(video);
		}
	}
}
