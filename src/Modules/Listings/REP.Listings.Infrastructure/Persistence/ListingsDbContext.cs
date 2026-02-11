using Microsoft.EntityFrameworkCore;
using REP.Listings.Application.Interfaces;
using REP.Listings.Domain.Aggregates;

namespace REP.Listings.Infrastructure.Persistence;

public class ListingsDbContext : DbContext, IListingsDbContext
{
    public ListingsDbContext(DbContextOptions<ListingsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Listing> Listings => Set<Listing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ListingsDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
