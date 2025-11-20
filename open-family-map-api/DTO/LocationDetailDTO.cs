namespace OpenFamilyMapAPI.DTO;

public class LocationDetailDTO
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime LocationUpdateDate { get; set; }

    public int UserId { get; set; }

    // Platform info
    public string Platform { get; set; } = "";   // "android" or "ios"
    public string? RawProvider { get; set; }     // Android provider or iOS source name

    // Core position
    public double Latitude { get; set; }         // degrees
    public double Longitude { get; set; }        // degrees

    // Altitude
    public double? AltitudeMeters { get; set; }          // WGS84 / sea-level-ish
    public double? VerticalAccuracyMeters { get; set; }  // Android VerticalAccuracyMeters / iOS verticalAccuracy

    // Horizontal accuracy
    public double? HorizontalAccuracyMeters { get; set; } // Android Accuracy / iOS horizontalAccuracy

    // Heading / bearing / course
    public double? BearingDegrees { get; set; }          // Android Bearing / iOS Course
    public double? BearingAccuracyDegrees { get; set; }  // Android BearingAccuracyDegrees / iOS courseAccuracy

    // Speed
    public double? SpeedMetersPerSecond { get; set; }          // Android Speed / iOS speed
    public double? SpeedAccuracyMetersPerSecond { get; set; }  // Android SpeedAccuracy / iOS speedAccuracy

    public TimeSpan? ElapsedRealtimeSinceBoot { get; set; }    // Android-only, from ElapsedRealtimeNanos

    // Indoor / extra info
    public int? FloorLevel { get; set; }                       // iOS CLLocation.floor?.level
    public bool? IsMock { get; set; }
}