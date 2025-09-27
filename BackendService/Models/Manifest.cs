namespace ExploreNasaBackendService.Models;
using System.Text.Json.Serialization;

public class ManifestResponse
{
    [JsonPropertyName("photo_manifest")]
    public required PhotoManifest PhotoManifest { get; set; }
}

public class PhotoManifest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("landing_date")]
    public required string LandingDate { get; set; }   // or DateOnly

    [JsonPropertyName("launch_date")]
    public required string LaunchDate { get; set; }    // or DateOnly

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("max_sol")]
    public int MaxSol { get; set; }

    [JsonPropertyName("max_date")]
    public required string MaxDate { get; set; }       // or DateOnly

    [JsonPropertyName("total_photos")]
    public int TotalPhotos { get; set; }

    [JsonPropertyName("photos")]
    public required List<ManifestPhoto> Photos { get; set; }
}

public class ManifestPhoto
{
    [JsonPropertyName("sol")]
    public int Sol { get; set; }

    [JsonPropertyName("earth_date")]
    public required string EarthDate { get; set; }     // or DateOnly

    [JsonPropertyName("total_photos")]
    public int TotalPhotos { get; set; }

    [JsonPropertyName("cameras")]
    public required List<string> Cameras { get; set; }
}

