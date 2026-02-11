using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using REP.Listings.Application.Interfaces;
using REP.Listings.Infrastructure.Persistence;
using REP.Listings.Infrastructure.Services;

namespace REP.Listings.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddListingsModule(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemory = configuration.GetValue<bool>("Database:UseInMemoryDatabase");

        if (useInMemory)
        {
            services.AddDbContext<ListingsDbContext>(options =>
                options.UseInMemoryDatabase("REP"));
        }
        else
        {
            var connectionString = configuration.GetValue<string>("Database:ConnectionString")
                ?? throw new InvalidOperationException("Database:ConnectionString is not configured.");

            services.AddDbContext<ListingsDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddScoped<IListingsDbContext>(sp => sp.GetRequiredService<ListingsDbContext>());
        services.AddScoped<IListingService, ListingService>();

        var storageConnectionString = configuration.GetValue<string>("AzureStorage:ConnectionString")
            ?? throw new InvalidOperationException("AzureStorage:ConnectionString is not configured.");
        var containerName = configuration.GetValue<string>("AzureStorage:ListingImagesContainer") ?? "listing-images";

        services.AddSingleton(_ => new BlobContainerClient(storageConnectionString, containerName));
        services.AddScoped<IPhotoStorageService, BlobPhotoStorageService>();

        return services;
    }

    public static async Task SeedListingsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ListingsDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var useInMemory = configuration.GetValue<bool>("Database:UseInMemoryDatabase");
        if (useInMemory)
        {
            await context.Database.EnsureCreatedAsync();
        }

        await ListingsDbContextSeed.SeedAsync(context);
    }
}
