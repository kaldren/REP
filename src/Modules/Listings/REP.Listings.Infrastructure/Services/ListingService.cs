using Microsoft.EntityFrameworkCore;
using REP.Listings.Application.Interfaces;
using REP.Listings.Application.Models;

namespace REP.Listings.Infrastructure.Services;

public class ListingService : IListingService
{
    private readonly IListingsDbContext _dbContext;

    public ListingService(IListingsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ListingResponse>> GetAllAsync()
    {
        return await _dbContext.Listings
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new ListingResponse(
                l.Id,
                l.Title,
                l.Description,
                l.Price != null ? l.Price.Amount : null,
                l.Price != null ? l.Price.Currency.ToString() : null,
                l.AuthorId,
                l.CreatedAt,
                l.UpdatedAt))
            .ToListAsync();
    }

    public async Task<ListingResponse?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Listings
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => new ListingResponse(
                l.Id,
                l.Title,
                l.Description,
                l.Price != null ? l.Price.Amount : null,
                l.Price != null ? l.Price.Currency.ToString() : null,
                l.AuthorId,
                l.CreatedAt,
                l.UpdatedAt))
            .FirstOrDefaultAsync();
    }
}
