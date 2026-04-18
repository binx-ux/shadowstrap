namespace Shadowstrap.Models
{
    internal class WatcherData
    {
        public int ProcessId { get; set; }

        public string? LogFile { get; set; }

        public List<int>? AutoclosePids { get; set; }

        // Original Roblox launch URL; used by auto-rejoin to relaunch the same session
        public string? LaunchUrl { get; set; }
    }
}
