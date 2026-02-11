using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using REP.Users.Application.Interfaces;
using REP.Users.Infrastructure.Persistence;
using REP.Users.Infrastructure.Services;

namespace REP.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemory = configuration.GetValue<bool>("Database:UseInMemoryDatabase");

        if (useInMemory)
        {
            services.AddDbContext<UsersDbContext>(options =>
                options.UseInMemoryDatabase("REP"));
        }
        else
        {
            var connectionString = configuration.GetValue<string>("Database:ConnectionString")
                ?? throw new InvalidOperationException("Database:ConnectionString is not configured.");

            services.AddDbContext<UsersDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddScoped<IUsersDbContext>(sp => sp.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static async Task SeedUsersAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var useInMemory = configuration.GetValue<bool>("Database:UseInMemoryDatabase");
        if (useInMemory)
        {
            await context.Database.EnsureCreatedAsync();
        }

        await UsersDbContextSeed.SeedAsync(context);
    }
}
