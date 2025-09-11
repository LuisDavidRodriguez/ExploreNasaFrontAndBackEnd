using System.Text.Json.Serialization;

namespace ExploreNasaBackendService.Models;
public class RoverPhotos
{
    [JsonPropertyName("photos")]
    public required IEnumerable<Photo> Photos { get; set; }
};

public class Photo
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    [JsonPropertyName("sol")]
    public int Sol { get; set; }
    [JsonPropertyName("camera")]
    public required Camera Camera { get; set; }
    [JsonPropertyName("img_src")]
    public required string ImgSrc { get; set; }
    [JsonPropertyName("earth_date")]
    public required string EarthDate { get; set; }
    [JsonPropertyName("rover")]
    public required Rover Rover { get; set; }
}

public class Camera
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("rover_id")]
    public required int RoverId { get; set; }
    [JsonPropertyName("full_name")]
    public required string FullName { get; set; }
};

public class Rover
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("landing_date")]
    public required string LandingDate { get; set; }
    [JsonPropertyName("launch_date")]
    public required string LaunchDate { get; set; }
    [JsonPropertyName("status")]
    public required string Status { get; set; }
}

