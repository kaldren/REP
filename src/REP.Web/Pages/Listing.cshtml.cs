using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REP.Identity.Application.Interfaces;
using REP.Listings.Application.Interfaces;
using REP.Listings.Application.Models;

namespace REP.Web.Pages;

public class ListingModel : PageModel
{
    private readonly IListingService _listingService;
    private readonly IUserService _userService;

    public ListingModel(IListingService listingService, IUserService userService)
    {
        _listingService = listingService;
        _userService = userService;
    }

    public ListingResponse Listing { get; set; } = default!;
    public string AuthorName { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var listing = await _listingService.GetByIdAsync(id);

        if (listing is null)
        {
            return NotFound();
        }

        Listing = listing;

        var user = await _userService.GetByIdAsync(listing.AuthorId);
        AuthorName = user?.Name ?? "Unknown";

        return Page();
    }
}
