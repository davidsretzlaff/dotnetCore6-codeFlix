using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Infra.Data.EF.Models;

namespace MyFlix.Catalog.Infra.Data.EF.Configurations
{
    internal class GenresCategoriesConfiguration : IEntityTypeConfiguration<GenresCategories>
    {
        public void Configure(EntityTypeBuilder<GenresCategories> builder)
            => builder.HasKey(relation => new 
            {
                relation.CategoryId,
                relation.GenreId
            });
    }
}
