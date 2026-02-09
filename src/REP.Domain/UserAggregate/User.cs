using REP.Domain.ListingAggregate;

namespace REP.Domain.UserAggregate;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Listing> Listings { get; set; }
}
