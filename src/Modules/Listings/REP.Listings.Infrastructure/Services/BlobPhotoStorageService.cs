using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using REP.Listings.Application.Interfaces;
using REP.Listings.Application.Models;

namespace REP.Listings.Infrastructure.Services;

public class BlobPhotoStorageService : IPhotoStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobPhotoStorageService(BlobContainerClient containerClient)
    {
        _containerClient = containerClient;
    }

    public async Task<IReadOnlyList<PhotoResponse>> ListAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        var prefix = $"{listingId}/";
        var photos = new List<PhotoResponse>();
        var sortOrder = 0;

        await foreach (var blob in _containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, prefix, cancellationToken))
        {
            var fileName = blob.Name[prefix.Length..];
            var blobClient = _containerClient.GetBlobClient(blob.Name);
            var url = GenerateSasUri(blobClient);

            photos.Add(new PhotoResponse(
                Id: Guid.Empty,
                FileName: fileName,
                Url: url,
                Description: null,
                SortOrder: sortOrder++));
        }

        return photos;
    }

    public async Task<string> UploadAsync(Guid listingId, string fileName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var blobPath = $"{listingId}/{fileName}";
        var blobClient = _containerClient.GetBlobClient(blobPath);

        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);

        return GenerateSasUri(blobClient);
    }

    public async Task DeleteAsync(Guid listingId, string fileName, CancellationToken cancellationToken = default)
    {
        var blobPath = $"{listingId}/{fileName}";
        var blobClient = _containerClient.GetBlobClient(blobPath);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteAllAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        var prefix = $"{listingId}/";

        await foreach (var blob in _containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, prefix, cancellationToken))
        {
            var blobClient = _containerClient.GetBlobClient(blob.Name);
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
    }

    private static string GenerateSasUri(BlobClient blobClient)
    {
        if (!blobClient.CanGenerateSasUri)
            return blobClient.Uri.ToString();

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        return blobClient.GenerateSasUri(sasBuilder).ToString();
    }
}
