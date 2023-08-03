using MyFlix.Catalog.Application.Interfaces;
using MyFlix.Catalog.Application.UseCases.Category.Common;
using MyFlix.Catalog.Domain.Repository;
using System;
using DomainEntity = MyFlix.Catalog.Domain.Entity;
namespace MyFlix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategory : ICreateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public CreateCategory(
        
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModelOutput> Handle(
            CreateCategoryInput input, 
            CancellationToken cancellationToken)
        {
            var category = new DomainEntity.Category(
                input.Name,
                input.Description,
                input.IsActive);

            await _categoryRepository.Insert(category, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return CategoryModelOutput.FromCategory(category);
        }
    }
}
