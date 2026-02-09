using REP.Domain.UserAggregate;

namespace REP.Domain.ListingAggregate;

public class Listing
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Price? Price { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
