using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyFlix.Catalog.Domain.Entity;

namespace MyFlix.Catalog.Infra.Data.EF.Configurations
{
    internal class GenreConfiguration: IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(genre => genre.Id);
        }
    }
}
