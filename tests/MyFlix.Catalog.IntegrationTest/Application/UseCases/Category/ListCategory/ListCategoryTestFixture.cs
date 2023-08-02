﻿using CodeFlix.Catalog.Domain.SeedWork.SearchableRepository;
using CodeFlix.Catalog.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;
using DomainEntity = CodeFlix.Catalog.Domain.Entity;
namespace CodeFlix.Catalog.IntegrationTest.Application.UseCases.Category.ListCategory
{

    [CollectionDefinition(nameof(ListCategoryTestFixture))]
    public class ListCategoryTestFixtureCollection : ICollectionFixture<ListCategoryTestFixture> { }
    public class ListCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public List<DomainEntity.Category> GetExampleCategoriesListWithNames(List<string> names)
          => names.Select(name =>
          {
              var category = GetExampleCategory();
              category.Update(name);
              return category;
          }).ToList();

        public List<DomainEntity.Category> CloneCategoriesListOrdered(
            List<DomainEntity.Category> categoriesList,
            string orderBy,
            SearchOrder order
        )
        {
            var listClone = new List<DomainEntity.Category>(categoriesList);
            var orderedEnumerable = (orderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.Name)
            };
            return orderedEnumerable.ToList();
        }
    }
}