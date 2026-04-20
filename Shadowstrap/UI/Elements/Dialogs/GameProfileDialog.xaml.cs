using System.Windows;
using Shadowstrap.Models.Persistable;

namespace Shadowstrap.UI.Elements.Dialogs
{
    public partial class GameProfileDialog
    {
        private readonly GameFlagProfile? _editing;
        private readonly bool _importMode;

        public GameFlagProfile? ResultProfile { get; private set; }

        // ── constructors ──────────────────────────────────────────────────────

        /// <summary>Add new profile.</summary>
        public GameProfileDialog()
        {
            InitializeComponent();
            FlagsBox.Text = "{\n  \"DFIntTaskSchedulerTargetFps\": \"9999\"\n}";
        }

        /// <summary>Edit existing profile.</summary>
        public GameProfileDialog(GameFlagProfile existing)
        {
            _editing = existing;
            InitializeComponent();
            NameBox.Text    = existing.Name;
            PlaceIdBox.Text = existing.PlaceId.ToString();
            FlagsBox.Text   = JsonSerializer.Serialize(existing.Flags,
                new JsonSerializerOptions { WriteIndented = true });
        }

        /// <summary>Import-mode: name + place ID pre-filled via JSON paste.</summary>
        public GameProfileDialog(bool importMode)
        {
            _importMode = importMode;
            InitializeComponent();
            Title = "Import Community Profile";
            FlagsBox.Text = "{\n  \"name\": \"My Profile\",\n  \"placeId\": 0,\n  \"flags\": {\n    \"DFIntTaskSchedulerTargetFps\": \"9999\"\n  }\n}";
        }

        // ── handlers ──────────────────────────────────────────────────────────

        private async void LookupBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!long.TryParse(PlaceIdBox.Text.Trim(), out long placeId))
            {
                ShowError("Enter a numeric Place ID first.");
                return;
            }

            LookupBtn.IsEnabled = false;
            try
            {
                // universe ID → game details
                var uni = await Http.GetJson<UniverseIdResponse>(
                    $"https://apis.roblox.com/universes/v1/places/{placeId}/universe");
                var details = await Http.GetJson<ApiArrayResponse<GameDetailResponse>>(
                    $"https://games.roblox.com/v1/games?universeIds={uni.UniverseId}");
                string gameName = details.Data?.FirstOrDefault()?.Name ?? "";
                if (!string.IsNullOrEmpty(gameName))
                    NameBox.Text = gameName;
            }
            catch
            {
                ShowError("Couldn't look up the game — check the Place ID.");
            }
            finally
            {
                LookupBtn.IsEnabled = true;
                ErrorText.Visibility = Visibility.Collapsed;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Visibility = Visibility.Collapsed;

            if (_importMode)
            {
                SaveImport();
                return;
            }

            string name = NameBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowError("Game name is required.");
                return;
            }

            if (!long.TryParse(PlaceIdBox.Text.Trim(), out long placeId) || placeId <= 0)
            {
                ShowError("Enter a valid numeric Place ID.");
                return;
            }

            Dictionary<string, string>? flags;
            try
            {
                flags = JsonSerializer.Deserialize<Dictionary<string, string>>(FlagsBox.Text.Trim());
                if (flags is null) throw new Exception();
            }
            catch
            {
                ShowError("Flags JSON is invalid. Must be { \"FlagName\": \"Value\" }.");
                return;
            }

            if (_editing is not null)
            {
                // mutate in-place — Settings will be saved by caller
                _editing.Name    = name;
                _editing.PlaceId = placeId;
                _editing.Flags   = flags;
                ResultProfile    = _editing;
            }
            else
            {
                ResultProfile = new GameFlagProfile
                {
                    Name    = name,
                    PlaceId = placeId,
                    Flags   = flags,
                    Enabled = true
                };
            }

            DialogResult = true;
        }

        private void SaveImport()
        {
            try
            {
                using var doc  = System.Text.Json.JsonDocument.Parse(FlagsBox.Text.Trim());
                var root       = doc.RootElement;

                string name    = root.TryGetProperty("name",    out var n) ? n.GetString() ?? "" : "";
                long   placeId = root.TryGetProperty("placeId", out var p) ? p.GetInt64()       : 0;
                var flags      = new Dictionary<string, string>();

                if (root.TryGetProperty("flags", out var f))
                    foreach (var kv in f.EnumerateObject())
                        flags[kv.Name] = kv.Value.GetString() ?? "";

                if (string.IsNullOrEmpty(name)) name = "Imported Profile";

                ResultProfile = new GameFlagProfile
                {
                    Name    = name,
                    PlaceId = placeId,
                    Flags   = flags,
                    Enabled = true
                };

                DialogResult = true;
            }
            catch
            {
                ShowError("Could not parse the import JSON. Check the format and try again.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) =>
            DialogResult = false;

        private void ShowError(string msg)
        {
            ErrorText.Text       = msg;
            ErrorText.Visibility = Visibility.Visible;
        }
    }
}
