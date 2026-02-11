using Microsoft.EntityFrameworkCore;
using REP.Users.Domain.Aggregates;

namespace REP.Users.Application.Interfaces;

public interface IUsersDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
