using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using Shadowstrap.Models.Persistable;
using Shadowstrap.UI.Elements.Dialogs;

namespace Shadowstrap.UI.Elements.Settings.Pages
{
    public partial class GameProfilesPage
    {
        private readonly ObservableCollection<GameProfileEntry> _builtIn = new();
        private readonly ObservableCollection<GameProfileEntry> _custom  = new();

        public GameProfilesPage()
        {
            InitializeComponent();
            BuiltInList.ItemsSource = _builtIn;
            UserList.ItemsSource    = _custom;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => Refresh();

        // ── setup ─────────────────────────────────────────────────────────────

        private void Refresh()
        {
            GlobalToggle.IsChecked = App.Settings.Prop.GameProfilesEnabled;

            _builtIn.Clear();
            foreach (var p in GameFlagProfile.BuiltIn)
            {
                var entry = new GameProfileEntry(p, isBuiltIn: true)
                {
                    IsEnabled = !App.Settings.Prop.DisabledBuiltInProfileIds.Contains(p.PlaceId)
                };
                _builtIn.Add(entry);
            }

            _custom.Clear();
            foreach (var p in App.Settings.Prop.UserGameProfiles)
            {
                var entry = new GameProfileEntry(p, isBuiltIn: false)
                {
                    IsEnabled = p.Enabled
                };
                _custom.Add(entry);
            }

            NoCustomText.Visibility = _custom.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

            _ = LoadThumbnailsAsync();
        }

        private async Task LoadThumbnailsAsync()
        {
            // batch all place IDs together
            var allEntries = _builtIn.Concat(_custom).ToList();
            if (!allEntries.Any()) return;

            var ids = string.Join(",", allEntries.Select(e => e.Profile.PlaceId).Distinct());
            try
            {
                var resp = await Http.GetJson<ThumbnailBatchResponse>(
                    $"https://thumbnails.roblox.com/v1/places/gameicons" +
                    $"?placeIds={ids}&returnPolicy=PlaceHolder&size=150x150&format=Png&isCircular=false");

                var map = resp.Data.ToDictionary(t => t.TargetId, t => t.ImageUrl ?? "");

                foreach (var entry in allEntries)
                {
                    if (map.TryGetValue(entry.Profile.PlaceId, out var url))
                        entry.ThumbnailUrl = url;
                }
            }
            catch { /* thumbnails are best-effort */ }
        }

        // ── toggle handlers ───────────────────────────────────────────────────

        private void GlobalToggle_Click(object sender, RoutedEventArgs e)
        {
            App.Settings.Prop.GameProfilesEnabled = GlobalToggle.IsChecked ?? true;
            App.Settings.Save();
        }

        private void ProfileToggle_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Wpf.Ui.Controls.ToggleSwitch toggle) return;
            if (toggle.Tag is not GameProfileEntry entry) return;

            bool enabled = toggle.IsChecked ?? false;
            entry.IsEnabled = enabled;

            if (entry.IsBuiltIn)
            {
                if (enabled)
                    App.Settings.Prop.DisabledBuiltInProfileIds.Remove(entry.Profile.PlaceId);
                else
                    App.Settings.Prop.DisabledBuiltInProfileIds.Add(entry.Profile.PlaceId);
            }
            else
            {
                entry.Profile.Enabled = enabled;
            }

            App.Settings.Save();
        }

        // ── custom profile CRUD ───────────────────────────────────────────────

        private void AddProfile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GameProfileDialog();
            if (dlg.ShowDialog() != true) return;

            App.Settings.Prop.UserGameProfiles.Add(dlg.ResultProfile!);
            App.Settings.Save();
            Refresh();
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.Button btn) return;
            if (btn.Tag is not GameProfileEntry entry) return;

            var dlg = new GameProfileDialog(entry.Profile);
            if (dlg.ShowDialog() != true) return;

            // the dialog edits the profile in-place
            App.Settings.Save();
            Refresh();
        }

        private void DeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.Button btn) return;
            if (btn.Tag is not GameProfileEntry entry) return;

            var result = Frontend.ShowMessageBox(
                $"Delete the profile \"{entry.Profile.Name}\"?",
                System.Windows.MessageBoxImage.Warning,
                System.Windows.MessageBoxButton.YesNo);

            if (result != System.Windows.MessageBoxResult.Yes) return;

            App.Settings.Prop.UserGameProfiles.RemoveAll(p => p.Id == entry.Profile.Id);
            App.Settings.Save();
            Refresh();
        }

        private void ImportProfile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GameProfileDialog(importMode: true);
            if (dlg.ShowDialog() != true) return;

            App.Settings.Prop.UserGameProfiles.Add(dlg.ResultProfile!);
            App.Settings.Save();
            Refresh();
        }

        // ── community ─────────────────────────────────────────────────────────

        private void BrowseCommunity_Click(object sender, RoutedEventArgs e) =>
            Utilities.ShellExecute("https://github.com/binx-ux/shadowstrap/tree/main/community");

        private void ShareFlags_Click(object sender, RoutedEventArgs e)
        {
            // Copy current FastFlags as a community profile JSON blob
            var json = JsonSerializer.Serialize(new
            {
                name        = "My Profile",
                description = "Add a description here",
                author      = "your-github-username",
                flags       = App.FastFlags.Prop
            }, new JsonSerializerOptions { WriteIndented = true });

            System.Windows.Clipboard.SetDataObject(json);
            Frontend.ShowMessageBox(
                "Your current FastFlags have been copied as a community profile JSON.\n\n" +
                "Open a PR to community/flags.json on GitHub to share them.",
                System.Windows.MessageBoxImage.Information);
        }
    }

    // ── display entry ──────────────────────────────────────────────────────────

    public class GameProfileEntry : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        private string? _thumbnailUrl;
        private bool    _isEnabled;

        public GameFlagProfile Profile   { get; }
        public bool            IsBuiltIn { get; }

        public string ThumbnailUrl
        {
            get => _thumbnailUrl ?? "";
            set { _thumbnailUrl = value; PropertyChanged?.Invoke(this, new(nameof(ThumbnailUrl))); }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set { _isEnabled = value; PropertyChanged?.Invoke(this, new(nameof(IsEnabled))); }
        }

        public string FlagsSummary =>
            $"{Profile.Flags.Count} flag{(Profile.Flags.Count == 1 ? "" : "s")}";

        public GameProfileEntry(GameFlagProfile profile, bool isBuiltIn)
        {
            Profile   = profile;
            IsBuiltIn = isBuiltIn;
        }
    }
}
