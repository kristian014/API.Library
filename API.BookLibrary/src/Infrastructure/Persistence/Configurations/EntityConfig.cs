using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure.Persistence.Configurations
{
    public class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder
                .HasMany(x => x.Books)
                .WithOne(x => x.Author)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }

    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .HasOne(x => x.Publisher)
                .WithMany(x => x.Books)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(e => e.Genre)
              .WithMany()
              .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(e => e.Status)
              .WithMany()
              .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(e => e.CoverType)
              .WithMany()
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
