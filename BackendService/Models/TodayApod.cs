namespace ExploreNasaBackendService.Models;
using System.Text.Json.Serialization;
public class ApodWithId
{
    public required string Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Url { get; set; } = String.Empty;
    public string Date { get; set; } = String.Empty;
    public string Explanation { get; set; } = String.Empty;
    public string MediaType { get; set; } = String.Empty;
}

public class TodayApod
{
    [JsonPropertyName("copyright")]
    public string Copyright { get; set; } = "";
    [JsonPropertyName("date")]
    public string Date { get; set; } = "";
    [JsonPropertyName("explanation")]
    public string Explanation { get; set; } = "";
    [JsonPropertyName("hdurl")]
    public string HdUrl { get; set; } = "";
    [JsonPropertyName("media_type")]
    public string MediaType { get; set; } = "";
    [JsonPropertyName("service_version")]
    public string ServiceVersion { get; set; } = "";
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";
    [JsonPropertyName("url")]
    public string Url { get; set; } = "";
}
