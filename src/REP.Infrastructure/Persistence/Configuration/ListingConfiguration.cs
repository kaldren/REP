using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REP.Domain.ListingAggregate;

namespace REP.Infrastructure.Persistence.Configuration;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.OwnsOne(x => x.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.Amount)
                .HasColumnName("PriceAmount")
                .HasColumnType("decimal(18,2)");

            priceBuilder.Property(p => p.Currency)
                .HasColumnName("PriceCurrency")
                .HasConversion<string>()
                .HasMaxLength(3);
        });
    }
}
