using Shadowstrap.Enums;

namespace Shadowstrap.UI.ViewModels.Settings
{
    public class BehaviourViewModel : NotifyPropertyChangedViewModel
    {
        public IReadOnlyDictionary<RobloxProcessPriority, string> ProcessPriorities { get; } = new Dictionary<RobloxProcessPriority, string>
        {
            { RobloxProcessPriority.Default,     "Default" },
            { RobloxProcessPriority.AboveNormal, "Above Normal" },
            { RobloxProcessPriority.High,        "High" },
            { RobloxProcessPriority.Realtime,    "Realtime (may cause stutters)" },
        };

        public RobloxProcessPriority SelectedProcessPriority
        {
            get => App.Settings.Prop.RobloxProcessPriority;
            set => App.Settings.Prop.RobloxProcessPriority = value;
        }

        public bool MultiInstanceEnabled
        {
            get => App.Settings.Prop.MultiInstanceEnabled;
            set => App.Settings.Prop.MultiInstanceEnabled = value;
        }

        public bool RamCleanerEnabled
        {
            get => App.Settings.Prop.RamCleanerEnabled;
            set => App.Settings.Prop.RamCleanerEnabled = value;
        }

        public bool ConfirmLaunches
        {
            get => App.Settings.Prop.ConfirmLaunches;
            set => App.Settings.Prop.ConfirmLaunches = value;
        }

        public bool BackgroundUpdates
        {
            get => App.Settings.Prop.BackgroundUpdatesEnabled;
            set => App.Settings.Prop.BackgroundUpdatesEnabled = value;
        }

        public bool IsRobloxInstallationMissing => !App.IsPlayerInstalled && !App.IsStudioInstalled;

        public bool ForceRobloxReinstallation
        {
            get => App.State.Prop.ForceReinstall || IsRobloxInstallationMissing;
            set => App.State.Prop.ForceReinstall = value;
        }
    }
}
