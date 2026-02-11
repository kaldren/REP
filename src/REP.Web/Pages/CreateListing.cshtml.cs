using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using REP.Listings.Application.Interfaces;
using REP.Listings.Domain.Aggregates;
using REP.Listings.Domain.Entities;
using REP.Listings.Domain.ValueObjects;

namespace REP.Web.Pages;

public class CreateListingModel : PageModel
{
    private readonly IPhotoStorageService _photoStorageService;
    private readonly IListingsDbContext _dbContext;

    public CreateListingModel(
        IPhotoStorageService photoStorageService,
        IListingsDbContext dbContext)
    {
        _photoStorageService = photoStorageService;
        _dbContext = dbContext;
    }

    [BindProperty]
    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [BindProperty]
    [Required, StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [BindProperty]
    [Range(0, double.MaxValue)]
    public decimal? PriceAmount { get; set; }

    [BindProperty]
    public string? PriceCurrency { get; set; }

    [BindProperty]
    public IFormFileCollection Photos { get; set; } = new FormFileCollection();

    public List<SelectListItem> Currencies { get; set; } = [];

    public void OnGet()
    {
        PopulateCurrencies();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        PopulateCurrencies();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Price? price = null;
        if (PriceAmount is not null && PriceCurrency is not null
            && Enum.TryParse<Currency>(PriceCurrency, ignoreCase: true, out var currency))
        {
            price = new Price(PriceAmount.Value, currency);
        }

        var now = DateTimeOffset.UtcNow;
        var listingId = Guid.NewGuid();

        // Use a placeholder AuthorId for now
        var authorId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001");

        var listing = new Listing
        {
            Id = listingId,
            Title = Title,
            Description = Description,
            Price = price,
            AuthorId = authorId,
            CreatedAt = now,
            UpdatedAt = now
        };

        var sortOrder = 0;
        foreach (var file in Photos)
        {
            if (file.Length == 0)
                continue;

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();
            var url = await _photoStorageService.UploadAsync(listingId, fileName, stream, file.ContentType);

            listing.Photos.Add(new Photo
            {
                Id = Guid.NewGuid(),
                ListingId = listingId,
                FileName = fileName,
                Url = url,
                SortOrder = sortOrder++,
                CreatedAt = now
            });
        }

        _dbContext.Listings.Add(listing);
        await _dbContext.SaveChangesAsync();

        return RedirectToPage("/Listing", new { id = listingId });
    }

    private void PopulateCurrencies()
    {
        Currencies = Enum.GetNames<Currency>()
            .Select(c => new SelectListItem(c, c))
            .ToList();
    }
}
