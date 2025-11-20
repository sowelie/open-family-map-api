using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenFamilyMapAPI.Entities;
using OpenFamilyMapAPI.DTO;
using OpenFamilyMapAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace OpenFamilyMapAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthController(UserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] UserLoginDTO loginInfo)
    {
        // find the user by the login
        User? user = _userRepository.FindByLogin(loginInfo.Login);

        var key = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }
        var result = Convert.ToBase64String(key);

        // compare the login (to ensure case sensitivity) and the password
        if (user != null && user.Login == loginInfo.Login && user.Password == loginInfo.Password)
        {
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        return Unauthorized();
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Login),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]
            ?? throw new InvalidOperationException("No JWT:SigningKey property has been specified")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"] ?? "https://localhost:7089",
            audience: _config["JWT:Audience"] ?? "https://localhost:7089",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}