using Microsoft.EntityFrameworkCore;
using REP.Listings.Domain.Aggregates;

namespace REP.Listings.Application.Interfaces;

public interface IListingsDbContext
{
    DbSet<Listing> Listings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
