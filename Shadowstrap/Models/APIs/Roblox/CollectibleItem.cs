namespace Shadowstrap.Models.APIs.Roblox
{
    public class CollectibleItem
    {
        [JsonPropertyName("userAssetId")]
        public long UserAssetId { get; set; }

        [JsonPropertyName("assetId")]
        public long AssetId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("recentAveragePrice")]
        public long RecentAveragePrice { get; set; }

        [JsonPropertyName("serialNumber")]
        public long? SerialNumber { get; set; }
    }
}
