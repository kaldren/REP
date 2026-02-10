using Microsoft.EntityFrameworkCore;
using REP.Listings.Domain.Aggregates;

namespace REP.Listings.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Listing> Listings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
