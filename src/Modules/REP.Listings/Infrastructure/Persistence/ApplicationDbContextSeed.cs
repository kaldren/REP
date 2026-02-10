using REP.Listings.Domain.Aggregates;
using REP.Listings.Domain.ValueObjects;

namespace REP.Listings.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    // Well-known User IDs matching Identity module seed data
    private static readonly Guid AliceId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001");
    private static readonly Guid BobId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002");

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Listings.Any())
            return;

        var now = DateTimeOffset.UtcNow;

        context.Listings.AddRange(
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Modern Apartment in City Center",
                Description = "A beautiful 2-bedroom apartment with panoramic city views, fully furnished and close to public transport.",
                Price = new Price(250_000m, Currency.EUR),
                AuthorId = AliceId,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Cozy Suburban Family Home",
                Description = "Spacious 4-bedroom house with a large garden, garage, and quiet neighborhood. Perfect for families.",
                Price = new Price(375_000m, Currency.EUR),
                AuthorId = BobId,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Studio Near the University",
                Description = "Compact studio apartment ideal for students. Walking distance to campus and local amenities.",
                Price = new Price(95_000m, Currency.USD),
                AuthorId = BobId,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Luxury Penthouse with Terrace",
                Description = "Top-floor penthouse featuring a private rooftop terrace, high-end finishes, and a concierge service.",
                Price = new Price(1_200_000m, Currency.GBP),
                AuthorId = AliceId,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Country Cottage Retreat",
                Description = "Charming stone cottage surrounded by countryside. Two bedrooms, open fireplace, and original features.",
                Price = null,
                AuthorId = AliceId,
                CreatedAt = now,
                UpdatedAt = now
            }
        );

        await context.SaveChangesAsync();
    }
}
