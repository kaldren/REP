using Microsoft.EntityFrameworkCore;
using REP.Domain.ListingAggregate;
using REP.Domain.UserAggregate;

namespace REP.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Listing> Listings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
