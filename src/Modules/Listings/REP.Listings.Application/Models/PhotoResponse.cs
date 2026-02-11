namespace REP.Listings.Application.Models;

public record PhotoResponse(
    Guid Id,
    string FileName,
    string Url,
    string? Description,
    int SortOrder);
