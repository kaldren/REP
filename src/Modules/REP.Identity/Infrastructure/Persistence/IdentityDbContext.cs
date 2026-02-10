using Microsoft.EntityFrameworkCore;
using REP.Identity.Application.Interfaces;
using REP.Identity.Domain.Aggregates;

namespace REP.Identity.Infrastructure.Persistence;

public class IdentityDbContext : DbContext, IIdentityDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
