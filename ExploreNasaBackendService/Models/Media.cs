using System.Text.Json.Serialization;
namespace ExploreNasaBackendService.Models;

public sealed class NasaSearchResponse
{
    [JsonPropertyName("collection")]
    public required Collection Collection { get; set; }
}

public sealed class Collection
{
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("items")]
    public List<Item>? Items { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }

    // top-level paging links (have "prompt")
    [JsonPropertyName("links")]
    public List<NavLink>? Links { get; set; }
}

public sealed class Item
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    // the “data” block (usually single element)
    [JsonPropertyName("data")]
    public List<ItemData>? Data { get; set; }

    // media renditions for this item
    [JsonPropertyName("links")]
    public List<ItemMediaLink>? Links { get; set; }
}

public sealed class ItemData
{
    [JsonPropertyName("center")]
    public string? Center { get; set; }

    [JsonPropertyName("date_created")]
    public DateTime? DateCreated { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("description_508")]
    public string? Description508 { get; set; }

    // NASA returns an array of strings; sometimes one string contains many comma-separated tags
    [JsonPropertyName("keywords")]
    public List<string>? Keywords { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    [JsonPropertyName("nasa_id")]
    public string? NasaId { get; set; }

    [JsonPropertyName("photographer")]
    public string? Photographer { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
}

public sealed class ItemMediaLink
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("rel")]
    public string? Rel { get; set; }

    [JsonPropertyName("render")]
    public string? Render { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }

    [JsonPropertyName("size")]
    public long? Size { get; set; }
}

public sealed class NavLink
{
    [JsonPropertyName("rel")]
    public string? Rel { get; set; }

    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; }

    [JsonPropertyName("href")]
    public string? Href { get; set; }
}

public sealed class Metadata
{
    [JsonPropertyName("total_hits")]
    public int? TotalHits { get; set; }
}
