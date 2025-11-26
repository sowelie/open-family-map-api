using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenFamilyMapAPI.Core.Data;
using OpenFamilyMapAPI.Entities;

namespace OpenFamilyMapAPI.Services;

public interface IJWTService
{
    string Generate(User user);
    Task<string> GenerateRefreshAsync(User user);
    Task<RefreshToken?> ValidateAsync(string token);
    Task RevokeAsync(string token);
}

public class JWTService(OpenFamilyMapContext db, IConfiguration config) : IJWTService
{
    private readonly OpenFamilyMapContext _db = db;
    private readonly IConfiguration _config = config;
    public readonly static TimeSpan TokenValidity = TimeSpan.FromDays(30);

    public async Task<string> GenerateRefreshAsync(User user)
    {
        // GUID + random bytes ≈ 256‑bit token
        var random = RandomNumberGenerator.GetBytes(32);
        var token = Convert.ToBase64String(random);

        var rt = new RefreshToken
        {
            Token = token,
            User = user,
            ExpiresAt = DateTime.UtcNow + TokenValidity
        };

        _db.RefreshTokens.Add(rt);
        await _db.SaveChangesAsync();
        return token;
    }

    public async Task<RefreshToken?> ValidateAsync(string token)
    {
        var rt = await _db.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (rt == null || rt.IsRevoked || rt.IsUsed || rt.ExpiresAt <= DateTime.UtcNow)
            return null;

        rt.IsUsed = true;
        await _db.SaveChangesAsync();

        return rt;
    }

    public async Task RevokeAsync(string token)
    {
        var rt = await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
        if (rt != null)
        {
            rt.IsRevoked = true;
            await _db.SaveChangesAsync();
        }
    }

    public string Generate(User user)
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