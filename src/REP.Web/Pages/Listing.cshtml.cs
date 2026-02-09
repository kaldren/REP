using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using REP.Application.Interfaces;
using REP.Domain.ListingAggregate;

namespace REP.Web.Pages;

public class ListingModel : PageModel
{
    private readonly IApplicationDbContext _dbContext;

    public ListingModel(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Listing Listing { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var listing = await _dbContext.Listings
            .Include(l => l.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

        if (listing is null)
        {
            return NotFound();
        }

        Listing = listing;
        return Page();
    }
}
