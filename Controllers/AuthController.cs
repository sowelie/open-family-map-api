using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenFamilyMapAPI.Entities;
using OpenFamilyMapAPI.DTO;
using OpenFamilyMapAPI.Repositories;

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
    public IActionResult Login([FromBody] UserLoginDTO loginInfo)
    {
        // find the user by the login
        User? user = _userRepository.FindByLogin(loginInfo.Login);

        // compare the login (to ensure case sensitivity) and the password
        if (user != null && user.Login == loginInfo.Login && user.Password == loginInfo.Password)
        {
            var token = GenerateJwtToken(user.Login);

            return Ok(new { token });
        }

        return Unauthorized();
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Domain"] ?? "localhost",
            audience: _config["Domain"] ?? "localhost",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}