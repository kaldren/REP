using Microsoft.EntityFrameworkCore;
using REP.Identity.Domain.Aggregates;

namespace REP.Identity.Application.Interfaces;

public interface IIdentityDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
