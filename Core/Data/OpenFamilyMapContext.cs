using System.Configuration;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenFamilyMapAPI.Entities;

namespace OpenFamilyMapAPI.Core.Data;

public class OpenFamilyMapContext : DbContext
{
    private readonly IEncryptionProvider _encryptionProvider;
    private readonly IConfiguration _configuration;

    public DbSet<User> Users { get; set; }

    public OpenFamilyMapContext(IConfiguration configuration)
    {
        _encryptionProvider = new GenerateEncryptionProvider(configuration["Database:EncryptionKey"]);
        _configuration = configuration;
    }

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