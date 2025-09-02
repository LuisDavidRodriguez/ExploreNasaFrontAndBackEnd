namespace ExploreNasaBackendService.Models
{
    public class TodayApod
    {
        public required string Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Url { get; set; } = String.Empty;
        public DateOnly Date { get; set; }
        public string Explanation { get; set; } = String.Empty;
        public MediaType MediaType { get; set; } = MediaType.Unknown;
    }
}
