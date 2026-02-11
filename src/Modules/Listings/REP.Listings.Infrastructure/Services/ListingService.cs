using Microsoft.EntityFrameworkCore;
using REP.Listings.Application.Interfaces;
using REP.Listings.Application.Models;
using REP.Listings.Domain.Aggregates;
using REP.Listings.Domain.ValueObjects;

namespace REP.Listings.Infrastructure.Services;

public class ListingService : IListingService
{
    private readonly IListingsDbContext _dbContext;

    public ListingService(IListingsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAsync(CreateListingRequest request, CancellationToken cancellationToken = default)
    {
        Price? price = null;
        if (request.PriceAmount is not null && request.PriceCurrency is not null
            && Enum.TryParse<Currency>(request.PriceCurrency, ignoreCase: true, out var currency))
        {
            price = new Price(request.PriceAmount.Value, currency);
        }

        var now = DateTimeOffset.UtcNow;
        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Price = price,
            AuthorId = request.AuthorId,
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.Listings.Add(listing);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return listing.Id;
    }

    public async Task<IReadOnlyList<ListingResponse>> GetAllAsync()
    {
        return await _dbContext.Listings
            .AsNoTracking()
            .Include(l => l.Photos)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new ListingResponse(
                l.Id,
                l.Title,
                l.Description,
                l.Price != null ? l.Price.Amount : null,
                l.Price != null ? l.Price.Currency.ToString() : null,
                l.AuthorId,
                l.CreatedAt,
                l.UpdatedAt,
                l.Photos
                    .OrderBy(p => p.SortOrder)
                    .Select(p => new PhotoResponse(p.Id, p.FileName, p.Url, p.Description, p.SortOrder))
                    .ToList()))
            .ToListAsync();
    }

    public async Task<ListingResponse?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Listings
            .AsNoTracking()
            .Include(l => l.Photos)
            .Where(l => l.Id == id)
            .Select(l => new ListingResponse(
                l.Id,
                l.Title,
                l.Description,
                l.Price != null ? l.Price.Amount : null,
                l.Price != null ? l.Price.Currency.ToString() : null,
                l.AuthorId,
                l.CreatedAt,
                l.UpdatedAt,
                l.Photos
                    .OrderBy(p => p.SortOrder)
                    .Select(p => new PhotoResponse(p.Id, p.FileName, p.Url, p.Description, p.SortOrder))
                    .ToList()))
            .FirstOrDefaultAsync();
    }
}
