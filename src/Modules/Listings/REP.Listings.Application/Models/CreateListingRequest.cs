namespace REP.Listings.Application.Models;

public record CreateListingRequest(
    string Title,
    string Description,
    decimal? PriceAmount,
    string? PriceCurrency,
    Guid AuthorId);
