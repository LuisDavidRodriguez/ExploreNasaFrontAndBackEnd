using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExploreNasaBackendService.Models;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace ExploreNasaBackendService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApodsController : ControllerBase
{
    private readonly DataBaseContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApodsController> _logger;
    public ApodsController(
        DataBaseContext context,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ApodsController> logger
        )
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Gets a list of APODs, projects the apods adding a Unique Id. The Id is regeterated at run time.
    /// Parameter | Type       | Default | Description
    /// date      | YYYY-MM-DD | today   | A string in YYYY-MM-DD format indicating the date of the APOD image (example: 2014-11-03). Defaults to today's date. Must be after 1995-06-16, the first day an APOD picture was posted. There are no images for tomorrow available through this API.
    /// start_date| YYYY-MM-DD | none    | the start of a date range, when requesting date for a range of dates. Cannot be used with date.
    /// end_date  | YYYY-MM-DD | today   | indicating that end of a date range. If start_date is specified without an end_date then end_date defaults to the current date.
    /// count     | int        | none    | A positive integer, no greater than 100. If this is specified then count randomly chosen images will be returned in a JSON array. Cannot be used in conjunction with date or start_date and end_date.
    /// thumbs    | bool       | False   | A boolean parameter True|False inidcating whether the API should return a thumbnail image URL for video files. If set to True, the API returns URL of video thumbnail. If an APOD is not a video, this parameter is ignored.
    /// api_key   | string     | DEMO_KEY| api.nasa.gov key for expanded usage
    /// </summary>
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<ApodWithId>>> Index()
    {

        var response = await HelperFetchData<IEnumerable<TodayApod>>(Request.QueryString.Value);
        if (response.Result != null)
        {
            return response.Result;
        }
        IEnumerable<ApodWithId> randoms = from apod in response.Value
                                          select new ApodWithId
                                          {
                                              Id = Guid.NewGuid().ToString(),
                                              Date = apod.Date,
                                              Explanation = apod.Explanation,
                                              MediaType = apod.MediaType,
                                              Title = apod.Title,
                                              Url = apod.Url,
                                          };

        return Ok(randoms);
    }

    // GET: api/Apods/GetTodayApodFromNasa
    [HttpGet("today-apod")]
    public async Task<ActionResult<TodayApod>> GetTodayApodFromNasa()
    {
        var response = await HelperFetchData<TodayApod>();
        return response;
    }

    // GET: api/Apods/GetTodayApodFromNasa
    [HttpGet("today-apod-test")]
    public async Task<ActionResult<TodayApod>> GetTodayApodFromNasaTest()
    {
        var handler = new HttpClientHandler
        {
            UseProxy = false
        };
        var client = new HttpClient(handler);
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var response = await client.GetAsync("https://api.nasa.gov/planetary/apod?api_key=ZF68DPHnWhdvWZRpqSUtvTfLOXMiwvOnHCQRLCZL");
        sw.Stop();
        _logger.LogInformation($"HTTP GET took {sw.ElapsedMilliseconds} ms");
        var content = await response.Content.ReadAsStringAsync();
        return Ok(content);
    }

    private String getBaseUrl()
    {
        string? baseUrl = _configuration["NasaApi:ApodBaseUrl"];
        string? apiKey = _configuration["NasaApi:ApiKey"];
        return $"{baseUrl}?api_key={apiKey}";
    }

    public async Task<ActionResult<T>> HelperFetchData<T>(String? queries = null)
    {
        var client = _httpClientFactory.CreateClient();
        string url = getBaseUrl();

        if (!string.IsNullOrEmpty(queries))
        {
            if (queries.StartsWith('?'))
                url = $"{url}&{queries.Substring(1)}";
            else
                url = $"{url}?{queries}";
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
