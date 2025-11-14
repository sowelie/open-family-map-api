namespace OpenFamilyMapAPI.DTO;

public abstract class BaseDTO {
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
}