using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REP.Users.Application.Interfaces;
using REP.Listings.Application.Interfaces;
using REP.Listings.Application.Models;

namespace REP.Web.Pages;

public class ListingModel : PageModel
{
    private readonly IListingService _listingService;
    private readonly IUserService _userService;
    private readonly IPhotoStorageService _photoStorageService;

    public ListingModel(IListingService listingService, IUserService userService, IPhotoStorageService photoStorageService)
    {
        _listingService = listingService;
        _userService = userService;
        _photoStorageService = photoStorageService;
    }

    public ListingResponse Listing { get; set; } = default!;
    public string AuthorName { get; set; } = string.Empty;
    public IReadOnlyList<PhotoResponse> Photos { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var listing = await _listingService.GetByIdAsync(id);

        if (listing is null)
        {
            return NotFound();
        }

        Listing = listing;
        Photos = await _photoStorageService.ListAsync(id);

        var user = await _userService.GetByIdAsync(listing.AuthorId);
        AuthorName = user?.Name ?? "Unknown";

        return Page();
    }
}
