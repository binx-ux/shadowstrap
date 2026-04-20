using System.Collections.ObjectModel;
using Shadowstrap.Enums;

namespace Shadowstrap.Models.Persistable
{
    public class Settings
    {
        // Shadowstrap configuration
        public BootstrapperStyle BootstrapperStyle { get; set; } = BootstrapperStyle.FluentDialog;
        public BootstrapperIcon BootstrapperIcon { get; set; } = BootstrapperIcon.IconShadowstrap;
        public string BootstrapperTitle { get; set; } = App.ProjectName;
        public string BootstrapperIconCustomLocation { get; set; } = "";
        public Theme Theme { get; set; } = Theme.Default;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool DeveloperMode { get; set; } = false;
        public bool CheckForUpdates { get; set; } = true;
        public bool ConfirmLaunches { get; set; } = false;
        public string Locale { get; set; } = "nil";
        public bool UseFastFlagManager { get; set; } = true;
        public bool WPFSoftwareRender { get; set; } = false;
        public bool EnableAnalytics { get; set; } = true;
        public bool BackgroundUpdatesEnabled { get; set; } = false;
        public bool DebugDisableVersionPackageCleanup { get; set; } = false;
        public string? SelectedCustomTheme { get; set; } = null;
        public WebEnvironment WebEnvironment { get; set; } = WebEnvironment.Production;

        // integration configuration
        public bool EnableActivityTracking { get; set; } = true;
        public bool UseDiscordRichPresence { get; set; } = true;
        public bool HideRPCButtons { get; set; } = true;
        public bool ShowAccountOnRichPresence { get; set; } = false;
        public bool ShowServerDetails { get; set; } = false;
        public ObservableCollection<CustomIntegration> CustomIntegrations { get; set; } = new();

        // mod preset configuration
        public bool UseDisableAppPatch { get; set; } = false;

        // hone.gg integration
        public bool UseHoneGG { get; set; } = false;
        public string HoneGGPath { get; set; } = "";

        // performance features
        public RobloxProcessPriority RobloxProcessPriority { get; set; } = RobloxProcessPriority.Default;
        public bool MultiInstanceEnabled { get; set; } = false;
        public bool RamCleanerEnabled { get; set; } = false;

        // auto-rejoin
        public bool AutoRejoinEnabled { get; set; } = false;

        // session summary popup shown after leaving a game
        public bool ShowSessionSummary { get; set; } = true;

        // last performance preset applied so the UI can highlight it on next open
        public PerformancePreset LastPerformancePreset { get; set; } = PerformancePreset.Default;

        // per-game FastFlag profiles
        public bool GameProfilesEnabled { get; set; } = true;
        public List<GameFlagProfile> UserGameProfiles { get; set; } = new();
        public HashSet<long> DisabledBuiltInProfileIds { get; set; } = new();

        // custom death sound (path stored so we can show current selection; the file lives in Modifications/)
        public string CustomDeathSoundPath { get; set; } = "";
    }
}
