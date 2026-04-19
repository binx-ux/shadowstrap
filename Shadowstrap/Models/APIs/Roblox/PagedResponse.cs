namespace Shadowstrap.Models.APIs.Roblox
{
    public class PagedResponse<T>
    {
        [JsonPropertyName("nextPageCursor")]
        public string? NextPageCursor { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
    }
}
