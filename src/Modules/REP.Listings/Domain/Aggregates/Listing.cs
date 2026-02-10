using REP.Listings.Domain.ValueObjects;

namespace REP.Listings.Domain.Aggregates;

public class Listing
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Price? Price { get; set; }
    public Guid AuthorId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
