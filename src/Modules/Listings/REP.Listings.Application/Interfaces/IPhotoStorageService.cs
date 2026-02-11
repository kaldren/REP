using REP.Listings.Application.Models;

namespace REP.Listings.Application.Interfaces;

public interface IPhotoStorageService
{
    Task<IReadOnlyList<PhotoResponse>> ListAsync(Guid listingId, CancellationToken cancellationToken = default);
    Task<string> UploadAsync(Guid listingId, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid listingId, string fileName, CancellationToken cancellationToken = default);
    Task DeleteAllAsync(Guid listingId, CancellationToken cancellationToken = default);
}
