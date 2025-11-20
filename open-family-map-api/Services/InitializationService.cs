using System.Threading.Tasks;
using OpenFamilyMapAPI.Core.Data;
using OpenFamilyMapAPI.Entities;
using OpenFamilyMapAPI.Repositories;

namespace OpenFamilyMapAPI.Services;

public class InitializationService
{
    private readonly UserRepository _userRepository;
    private readonly IConfiguration _config;

    public InitializationService(UserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _config = configuration;
    }

    public async Task Initialize()
    {
        // check to see if the admin user has been created
        User user = _userRepository.FindByLogin("admin") ?? new User();

        user.DisplayName = _config["AdminUser:DisplayName"] ?? "Admin user";
        user.Login = _config["AdminUser:Login"] ?? "admin";
        user.Password = _config["AdminUser:Password"] ?? throw new InvalidOperationException("The admin user's password must be specified using the OPENFAMILYMAP_ADMINUSER__PASSWORD environment variable.");
        user.IsAdmin = true;

        await _userRepository.UpdateAsync(user);
    }
}
