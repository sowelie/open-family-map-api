using System.ComponentModel.DataAnnotations;

namespace OpenFamilyMapAPI.Entities;

public class User : BaseEntity
{
    [Required]
    public string Login { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    [Required]
    public string DisplayName { get; set; } = "";

    public ICollection<LocationDetail> LocationHistory { get; set; } = null!;
}