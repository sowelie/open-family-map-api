using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using OpenFamilyMapAPI.Entities;

namespace OpenFamilyMapAPI.Core.Data;

public class OpenFamilyMapContext : DbContext
{
    private readonly IEncryptionProvider _encryptionProvider;

    public DbSet<User> Users { get; set; }

    public OpenFamilyMapContext(DbContextOptions<OpenFamilyMapContext> options) : base(options)
    {
        _encryptionProvider = new GenerateEncryptionProvider("16a9af95f221cc7f2189921d936ccf9e");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("server=10.0.0.5;database=open-family-map;user=open-family-map;password=dXuv7Bdi291bn6");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseEncryption(_encryptionProvider);
        modelBuilder.Entity<User>()
            .HasData(
                new User {
                    Id = 1,
                    CreatedDate = DateTime.Parse("2025-11-09 00:00:00"),
                    UpdatedDate = DateTime.Parse("2025-11-09 00:00:00"),
                    DisplayName = "Admin User",
                    Login = "admin",
                    Password = "admin"
                }
            );
    }
}