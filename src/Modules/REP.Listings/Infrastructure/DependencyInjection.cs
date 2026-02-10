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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("REP"));
        }
        else
        {
            var connectionString = configuration.GetValue<string>("Database:ConnectionString")
                ?? throw new InvalidOperationException("Database:ConnectionString is not configured.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IListingService, ListingService>();

        return services;
    }

    public static async Task SeedListingsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var useInMemory = configuration.GetValue<bool>("Database:UseInMemoryDatabase");
        if (useInMemory)
        {
            await context.Database.EnsureCreatedAsync();
        }

        await ApplicationDbContextSeed.SeedAsync(context);
    }
}
