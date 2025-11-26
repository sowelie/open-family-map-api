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
using OpenFamilyMapAPI.Services;
using System.Threading.Tasks;

namespace OpenFamilyMapAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(UserRepository userRepository, IJWTService jwtService) : ControllerBase
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly IJWTService _jwtService = jwtService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginInfo)
    {
        // find the user by the login
        User? user = _userRepository.FindByLogin(loginInfo.Login);

        var key = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }

        // compare the login (to ensure case sensitivity) and the password
        if (user != null && user.Login == loginInfo.Login && user.Password == loginInfo.Password)
        {
            var token = _jwtService.Generate(user);
            var refreshToken = await _jwtService.GenerateRefreshAsync(user);

            SetCookie(refreshToken);

            return Ok(new { accessToken = token, refreshToken });
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
    {
        // Accept the token from a cookie or body
        var token = req.RefreshToken ?? Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token)) return Unauthorized();

        var rt = await _jwtService.ValidateAsync(token);
        if (rt == null) return Unauthorized();

        // Rotate – issue brand new tokens
        var newAccess  = _jwtService.Generate(rt.User);
        var newRefresh = await _jwtService.GenerateRefreshAsync(rt.User);

        SetCookie(newRefresh);

        return Ok(new { accessToken = newAccess, refreshToken = newRefresh });   // or just return the access token and cookie
    }

    private void SetCookie(string newRefresh)
    {
        // Set new cookie (overwrite)
        Response.Cookies.Append(
            "refreshToken",
            newRefresh,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.Add(JWTService.TokenValidity)
            });
    }
}