namespace Shadowstrap.Models.APIs.Roblox
{
    public class ServerInstance
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonPropertyName("playing")]
        public int Playing { get; set; }

        [JsonPropertyName("fps")]
        public double Fps { get; set; }

        [JsonPropertyName("ping")]
        public int Ping { get; set; }

        // Display helpers for ListView binding
        public string PlayerDisplay => $"{Playing} / {MaxPlayers}";
        public string FpsDisplay   => Fps  > 0 ? $"{(int)Fps}" : "—";
        public string PingDisplay  => Ping > 0 ? $"{Ping} ms"  : "—";
        public bool   IsFull       => Playing >= MaxPlayers;

        public string Region => Ping switch
        {
            <= 0  => "Unknown",
            < 60  => "NA East",
            < 100 => "NA West / EU",
            < 140 => "EU / Middle East",
            < 200 => "Asia / SA",
            _     => "Oceania / Asia"
        };
    }
}
