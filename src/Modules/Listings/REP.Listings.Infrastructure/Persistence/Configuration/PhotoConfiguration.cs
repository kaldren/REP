using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REP.Listings.Domain.Entities;

namespace REP.Listings.Infrastructure.Persistence.Configuration;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasOne<Domain.Aggregates.Listing>()
            .WithMany(l => l.Photos)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
