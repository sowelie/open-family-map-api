using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenFamilyMapAPI.Core.Data;
using OpenFamilyMapAPI.Entities;
using OpenFamilyMapAPI.Repositories;

namespace OpenFamilyMapAPI.Services;

public class InitializationService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    private readonly IConfiguration _config = configuration;
    private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OpenFamilyMapContext>();
        var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();

        await db.Database.MigrateAsync();

        // check to see if the admin user has been created
        User user = userRepository.FindByLogin("admin") ?? new User();

        user.DisplayName = _config["AdminUser:DisplayName"] ?? "Admin user";
        user.Login = _config["AdminUser:Login"] ?? "admin";
        user.Password = _config["AdminUser:Password"] ?? throw new InvalidOperationException("The admin user's password must be specified using the OPENFAMILYMAP_ADMINUSER__PASSWORD environment variable.");
        user.IsAdmin = true;

        await userRepository.UpdateAsync(user);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
