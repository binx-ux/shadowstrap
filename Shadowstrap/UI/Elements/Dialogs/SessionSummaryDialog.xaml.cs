using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Shadowstrap.AppData;
using Shadowstrap.Models.Entities;
using Shadowstrap.UI.ViewModels;

namespace Shadowstrap.UI.Elements.Dialogs
{
    public partial class SessionSummaryDialog
    {
        public SessionSummaryDialog(ActivityData data)
        {
            DataContext = new SessionSummaryViewModel(data);
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }

    internal class SessionSummaryViewModel : NotifyPropertyChangedViewModel
    {
        private readonly ActivityData _data;

        public SessionSummaryViewModel(ActivityData data)
        {
            _data = data;
            _ = LoadRegionAsync();
        }

        public string GameName    => _data.UniverseDetails?.Data.Name       ?? $"Place {_data.PlaceId}";
        public string CreatorName => _data.UniverseDetails?.Data.Creator.Name ?? string.Empty;
        public string ThumbnailUrl=> _data.UniverseDetails?.Thumbnail.ImageUrl ?? string.Empty;

        public string Duration
        {
            get
            {
                if (_data.TimeLeft is null) return "—";
                var span = _data.TimeLeft.Value - _data.TimeJoined;
                if (span.TotalHours >= 1)
                    return $"{(int)span.TotalHours}h {span.Minutes}m";
                if (span.TotalMinutes >= 1)
                    return $"{span.Minutes}m {span.Seconds}s";
                return $"{span.Seconds}s";
            }
        }

        public string TimeJoined => _data.TimeJoined.ToString("t");
        public string TimeLeft   => _data.TimeLeft?.ToString("t") ?? "—";

        private string _region = string.Empty;
        public string Region
        {
            get => _region;
            private set { _region = value; OnPropertyChanged(nameof(Region)); OnPropertyChanged(nameof(HasRegion)); }
        }
        public bool HasRegion => !string.IsNullOrEmpty(_region);

        public bool CanRejoin => !string.IsNullOrEmpty(_data.JobId);

        public ICommand RejoinCommand => new RelayCommand(Rejoin);

        private void Rejoin()
        {
            string deeplink = _data.GetInviteDeeplink(false);
            try
            {
                Process.Start(new RobloxPlayerData().ExecutablePath, deeplink);
            }
            catch
            {
                Process.Start(new ProcessStartInfo { FileName = deeplink, UseShellExecute = true });
            }
        }

        private async Task LoadRegionAsync()
        {
            if (!_data.MachineAddressValid) return;
            try
            {
                string? loc = await _data.QueryServerLocation();
                if (!string.IsNullOrEmpty(loc))
                    Region = loc;
            }
            catch { }
        }
    }
}
