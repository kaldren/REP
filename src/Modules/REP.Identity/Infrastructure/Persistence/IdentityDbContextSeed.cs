using REP.Identity.Domain.Aggregates;

namespace REP.Identity.Infrastructure.Persistence;

public static class IdentityDbContextSeed
{
    public static readonly Guid AliceId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001");
    public static readonly Guid BobId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002");

    public static async Task SeedAsync(IdentityDbContext context)
    {
        if (context.Users.Any())
            return;

        context.Users.AddRange(
            new User
            {
                Id = AliceId,
                Name = "Alice Johnson",
                Email = "alice.johnson@example.com"
            },
            new User
            {
                Id = BobId,
                Name = "Bob Smith",
                Email = "bob.smith@example.com"
            }
        );

        await context.SaveChangesAsync();
    }
}
