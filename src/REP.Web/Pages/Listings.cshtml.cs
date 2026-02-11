using Microsoft.AspNetCore.Mvc.RazorPages;
using REP.Users.Application.Interfaces;
using REP.Listings.Application.Interfaces;
using REP.Listings.Application.Models;

namespace REP.Web.Pages;

public class ListingsModel : PageModel
{
    private readonly IListingService _listingService;
    private readonly IUserService _userService;

    public ListingsModel(IListingService listingService, IUserService userService)
    {
        _listingService = listingService;
        _userService = userService;
    }

    public IReadOnlyList<ListingResponse> Listings { get; set; } = [];
    public IReadOnlyDictionary<Guid, string> AuthorNames { get; set; } = new Dictionary<Guid, string>();

    public async Task OnGetAsync()
    {
        Listings = await _listingService.GetAllAsync();

        var authorIds = Listings.Select(l => l.AuthorId).Distinct();
        AuthorNames = await _userService.GetNamesByIdsAsync(authorIds);
    }
}
