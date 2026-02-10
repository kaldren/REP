using Microsoft.EntityFrameworkCore;
using REP.Identity.Application.Interfaces;
using REP.Identity.Application.Models;

namespace REP.Identity.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IIdentityDbContext _dbContext;

    public UserService(IIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserResponse(u.Id, u.Name, u.Email))
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyDictionary<Guid, string>> GetNamesByIdsAsync(IEnumerable<Guid> ids)
    {
        var idList = ids.ToList();

        return await _dbContext.Users
            .AsNoTracking()
            .Where(u => idList.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.Name);
    }
}
