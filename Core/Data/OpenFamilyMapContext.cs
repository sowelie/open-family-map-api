using Microsoft.EntityFrameworkCore;

namespace OpenFamilyMapAPI.Core.Data;

public class OpenFamilyMapContext : DbContext
{
    public DbSet<User> Users { get; set; }
}