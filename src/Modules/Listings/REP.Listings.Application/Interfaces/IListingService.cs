using REP.Listings.Application.Models;

namespace REP.Listings.Application.Interfaces;

public interface IListingService
{
    Task<IReadOnlyList<ListingResponse>> GetAllAsync();
    Task<ListingResponse?> GetByIdAsync(Guid id);
}
