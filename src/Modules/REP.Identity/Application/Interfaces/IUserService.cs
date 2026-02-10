using REP.Identity.Application.Models;

namespace REP.Identity.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse?> GetByIdAsync(Guid id);
    Task<IReadOnlyDictionary<Guid, string>> GetNamesByIdsAsync(IEnumerable<Guid> ids);
}
