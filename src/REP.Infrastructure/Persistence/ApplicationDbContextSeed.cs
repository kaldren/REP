using REP.Domain.ListingAggregate;
using REP.Domain.UserAggregate;

namespace REP.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Listings.Any())
            return;

        var now = DateTimeOffset.UtcNow;

        var alice = new User
        {
            Id = Guid.NewGuid(),
            Name = "Alice Johnson",
            Email = "alice.johnson@example.com"
        };

        var bob = new User
        {
            Id = Guid.NewGuid(),
            Name = "Bob Smith",
            Email = "bob.smith@example.com"
        };

        context.Users.AddRange(alice, bob);

        context.Listings.AddRange(
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Modern Apartment in City Center",
                Description = "A beautiful 2-bedroom apartment with panoramic city views, fully furnished and close to public transport.",
                Price = new Price(250_000m, Currency.EUR),
                UserId = alice.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Cozy Suburban Family Home",
                Description = "Spacious 4-bedroom house with a large garden, garage, and quiet neighborhood. Perfect for families.",
                Price = new Price(375_000m, Currency.EUR),
                UserId = bob.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Studio Near the University",
                Description = "Compact studio apartment ideal for students. Walking distance to campus and local amenities.",
                Price = new Price(95_000m, Currency.USD),
                UserId = bob.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Luxury Penthouse with Terrace",
                Description = "Top-floor penthouse featuring a private rooftop terrace, high-end finishes, and a concierge service.",
                Price = new Price(1_200_000m, Currency.GBP),
                UserId = alice.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Listing
            {
                Id = Guid.NewGuid(),
                Title = "Country Cottage Retreat",
                Description = "Charming stone cottage surrounded by countryside. Two bedrooms, open fireplace, and original features.",
                Price = null,
                UserId = alice.Id,
                CreatedAt = now,
                UpdatedAt = now
            }
        );

        await context.SaveChangesAsync();
    }
}
