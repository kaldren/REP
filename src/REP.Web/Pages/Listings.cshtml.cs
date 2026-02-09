using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using REP.Application.Interfaces;
using REP.Domain.ListingAggregate;

namespace REP.Web.Pages;

public class ListingsModel : PageModel
{
    private readonly IApplicationDbContext _dbContext;

    public ListingsModel(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<Listing> Listings { get; set; } = [];

    public async Task OnGetAsync()
    {
        Listings = await _dbContext.Listings
            .Include(l => l.User)
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }
}
