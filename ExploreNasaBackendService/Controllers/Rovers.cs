using ExploreNasaBackendService.Models;
using Humanizer;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Linq;

namespace ExploreNasaBackendService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Rovers : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Rovers> _logger;

    public Rovers(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<Rovers> logger
        )
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    ///     Querying by Martian sol
    /// Parameter | Type   | Default | Description
    /// sol       | int    | none    | sol(ranges from 0 to max found in endpoint)
    /// camera    | string | all     | see table above for abbreviations
    /// page      | int    | 1       | 25 items per page returned
    /// rover     |string  | Curiosity | 
    /// 
    /// Querying by Earth date
    /// Parameter  |Type       | Default  | Description
    /// earth_date |YYYY-MM-DD |none      |corresponding date on earth for the given sol
    /// camera     |string     |all       |see table above for abbreviations
    /// page       |int        |1         |25 items per page returned
    /// rover     |string  | Curiosity |
    /// </summary>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<ActionResult<RoverPhotos>> Index()
    {
        var (rover, rest) = getStringifiedParametersAndRover(Request.Query);
        string photosUrl = getPhotosBaseUrl(rover);
        var result = await HelperFetchData<RoverPhotos>(photosUrl, rest);
        return result;
    }

    /// <summary>
    /// Mission Manifest
    ///    A mission manifest is available for each Rover at /manifests/rover_name.
    ///    This manifest will list details of the Rover's mission to help narrow down
    /// photo queries to the API.The information in the manifest includes:
    ///
    /// Key Description
    /// name Name of the Rover
    /// landing_date The Rover's landing date on Mars
    /// launch_date The Rover's launch date from Earth
    /// status The Rover's mission status
    /// max_sol The most recent Martian sol from which photos exist
    /// max_date The most recent Earth date from which photos exist
    /// total_photos Number of photos taken by that Rover
    /// It also includes a list of objects under the "photos"
    /// key which are grouped by sol, and each of which contains:
    /// 
    /// Key Description
    /// sol Martian sol of the Rover's mission
    /// total_photos Number of photos taken by that Rover on that sol
    /// cameras Cameras for which there are photos by that Rover on that sol
    /// An example entry from a sol at /manifests/Curiosity might look like:
    /// 
    /// {sol: 0, total_photos: 3702, cameras: ["CHEMCAM", "FHAZ", "MARDI", "RHAZ"]}
    /// 
    /// This would tell you that this rover, on sol 0, took 3702 photos,
    /// and those are from among the CHEMCAM, FHAZ, MARDI, and RHAZ cameras.
    /// </summary>
    /// <returns></returns>
    [HttpGet("Manifest")]
    public async Task<ActionResult<ManifestResponse>> Manifest()
    {
        var (rover, rest) = getStringifiedParametersAndRover(Request.Query);
        string manifest = getManifestBaseUrl(rover);
        var result = await HelperFetchData<ManifestResponse>(manifest, rest);
        return result;
    }

    private (String rover, String rest) getStringifiedParametersAndRover(IQueryCollection query)
    {
        string rover = query.ContainsKey("rover") ? query["rover"].ToString().ToLower() : "curiosity";
        var restParameters = query.Where(a => a.Key != "rover").Select(kvp => $"{kvp.Key}={kvp.Value}");
        string rest = string.Join("&", restParameters);

        return (rover, rest);
    }

    private String getPhotosBaseUrl(string rover)
    {
        string? baseUrl = _configuration["NasaApi:RoverPhotosBaseUrl"];
        string? apiKey = _configuration["NasaApi:ApiKey"];
        if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiKey)) return "";
        string formedUrl = baseUrl.Replace("{{rover}}", rover).Replace("{{key}}", apiKey);
        return formedUrl;
    }

    private String getManifestBaseUrl(string rover)
    {
        string? baseUrl = _configuration["NasaApi:RoverManifestBaseUrl"];
        string? apiKey = _configuration["NasaApi:ApiKey"];
        if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiKey)) return "";
        string formedUrl = baseUrl.Replace("{{rover}}", rover).Replace("{{key}}", apiKey);
        return formedUrl;
    }


    public async Task<ActionResult<T>> HelperFetchData<T>(String url, String? queries = null)
    {
        var client = _httpClientFactory.CreateClient();

        if (!string.IsNullOrEmpty(queries))
        {
            if (queries.StartsWith('?'))
                url = $"{url}&{queries.Substring(1)}";
            else
                url = $"{url}&{queries}";
        }
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync(url);
        sw.Stop();
        _logger.LogInformation($"HTTP GET to NASA API took {sw.ElapsedMilliseconds} ms");

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Error fetching data from NASA API");
        }

        sw.Restart();
        var data = await response.Content.ReadFromJsonAsync<T>();
        sw.Stop();
        _logger.LogInformation($"Deserialization took {sw.ElapsedMilliseconds} ms");

        if (data == null)
        {
            return StatusCode(500, "Error parsing data from NASA API");
        }
        return data;
    }
}
