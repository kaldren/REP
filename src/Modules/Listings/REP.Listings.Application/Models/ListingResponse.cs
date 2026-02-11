namespace REP.Listings.Application.Models;

public record ListingResponse(
    Guid Id,
    string Title,
    string Description,
    decimal? PriceAmount,
    string? PriceCurrency,
    Guid AuthorId,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    IReadOnlyList<PhotoResponse> Photos);
