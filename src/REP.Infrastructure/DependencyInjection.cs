using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using REP.Application.Interfaces;
using REP.Infrastructure.Persistence;

namespace REP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
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

        return services;
    }

    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
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
