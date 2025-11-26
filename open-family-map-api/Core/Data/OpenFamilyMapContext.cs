using System.Configuration;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenFamilyMapAPI.Entities;

namespace OpenFamilyMapAPI.Core.Data;

public class OpenFamilyMapContext(DbContextOptions<OpenFamilyMapContext> options, IConfiguration configuration) : DbContext(options)
{
    private readonly IEncryptionProvider _encryptionProvider = new GenerateEncryptionProvider(configuration["Database:EncryptionKey"]);
    private readonly IConfiguration _configuration = configuration;

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<LocationDetail> LocationDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseMySQL($"server={_configuration["Database:Host"]};database={_configuration["Database:Name"]};user={_configuration["Database:User"]};password={_configuration["Database:Password"]}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseEncryption(_encryptionProvider);
    }
}