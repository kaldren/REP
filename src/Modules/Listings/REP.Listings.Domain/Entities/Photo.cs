namespace REP.Listings.Domain.Entities;

public class Photo
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public string Url { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
