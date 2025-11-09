using EntityFrameworkCore.EncryptColumn.Attribute;

namespace OpenFamilyMapAPI.Entities;

public class User : BaseEntity
{
    public string Login { get; set; } = "";

    [EncryptColumn]
    public string Password { get; set; } = "";

    public string DisplayName { get; set; } = "";

    public ICollection<LocationDetail> LocationHistory { get; set; } = null!;
}