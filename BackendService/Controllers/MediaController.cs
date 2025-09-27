using ExploreNasaBackendService.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Text.Encodings.Web;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExploreNasaBackendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MediaController> _logger;

        public MediaController (
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<MediaController> logger
        )
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }
        /// <summary>
        ///     https://images.nasa.gov/docs/images.nasa.gov_api_docs.pdf
        ///    Name Type      Description
        ///    q(optional)                          string Free text search terms to compare to all indexed metadata.
        ///    center(optional) string NASA center which published the media.
        ///    description(optional) string Terms to search for in “Description” fields.
        ///    description_508(optional) string Terms to search for in “508 Description” fields.
        ///    keywords(optional) string Terms to search for in “Keywords” fields.Separate multiple values with commas.
        ///    location(optional) string Terms to search for in “Location” fields.
        ///    media_type(optional) string Media types to restrict the search to.Available types: [“image”, “audio”]. Separate multiple values with commas.
        ///    nasa_id (optional)                    string The media asset’s NASA ID.
        ///    page (optional) integer    Page number, starting at 1, of results to get.
        ///
        ///    photographer(optional)                string The primary photographer’s name.
        ///
        ///    secondary_creator(optional)           string A secondary photographer/videographer’s name.
        ///
        ///    title (optional)                      string Terms to search for in “Title” fields.
        ///    year_start (optional)                 string The start year for results.Format: YYYY.
        ///    year_end (optional)                   string The end year for results.Format: YYYY.
        ///
        ///    Example Request:
        ///
        ///    At least one parameter is required, but all individual parameters are optional. All parameter values must be URL­encoded. Most
        ///    HTTP client libraries will take care of this for you.Use --data-urlencode to encode values using curl:
        ///    "https://images-api.nasa.gov/search
        ///    ? q = apollo % 2011
        ///    & description = moon % 20landing
        ///    &media_type=image" |
        ///    python -m json.tool
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<ActionResult<NasaSearchResponse>> Index()
        {
            string baseUrl = _configuration["NasaApi:MediaBaseUrl"] ?? "";
            string url = $"{baseUrl}{Request.QueryString.Value}";
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching data from NASA API");
            }

            var data = await response.Content.ReadFromJsonAsync<NasaSearchResponse>();
            if (data == null)
            {
                return StatusCode(500, "Error parsing data from NASA API");
            }
            return data;
        }
    }
}
