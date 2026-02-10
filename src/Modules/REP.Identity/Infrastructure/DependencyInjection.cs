using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using REP.Identity.Application.Interfaces;
using REP.Identity.Infrastructure.Persistence;
using REP.Identity.Infrastructure.Services;

namespace REP.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemory = configuration.GetValue<bool>("Database:UseInMemoryDatabase");

        if (useInMemory)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseInMemoryDatabase("REP"));
        }
        else
        {
            var connectionString = configuration.GetValue<string>("Database:ConnectionString")
                ?? throw new InvalidOperationException("Database:ConnectionString is not configured.");

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddScoped<IIdentityDbContext>(sp => sp.GetRequiredService<IdentityDbContext>());
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static async Task SeedIdentityAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var useInMemory = configuration.GetValue<bool>("Database:UseInMemoryDatabase");
        if (useInMemory)
        {
            await context.Database.EnsureCreatedAsync();
        }

        await IdentityDbContextSeed.SeedAsync(context);
    }
}
