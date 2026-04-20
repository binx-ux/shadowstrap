using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Shadowstrap.UI.Elements.Settings.Pages
{
    public partial class InventoryPage
    {
        private readonly ObservableCollection<InventoryDisplayItem> _items = new();
        private long   _currentUserId;
        private string _currentUsername = string.Empty;
        private string? _nextCursor;

        public InventoryPage()
        {
            InitializeComponent();
            ItemsControl.ItemsSource = _items;
        }

        // ── event handlers ────────────────────────────────────────────────────

        private async void SearchButton_Click(object sender, RoutedEventArgs e) =>
            await RunLookup();

        private async void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) await RunLookup();
        }

        private async void LoadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            LoadMoreButton.IsEnabled = false;
            try   { await FetchPage(); }
            catch (Exception ex) { ShowError($"Failed to load more: {ex.Message}"); }
            finally { LoadMoreButton.IsEnabled = true; }
        }

        // ── core logic ────────────────────────────────────────────────────────

        private async Task RunLookup()
        {
            string input = UsernameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            // reset UI
            _items.Clear();
            _nextCursor = null;
            SummaryBorder.Visibility  = Visibility.Collapsed;
            ItemsControl.Visibility   = Visibility.Collapsed;
            ErrorPanel.Visibility     = Visibility.Collapsed;
            LoadMoreButton.Visibility = Visibility.Collapsed;
            LoadingPanel.Visibility   = Visibility.Visible;
            SearchButton.IsEnabled    = false;

            try
            {
                // resolve user — numeric ID or username
                if (long.TryParse(input, out long numId))
                {
                    _currentUserId   = numId;
                    _currentUsername = $"User {numId}";
                    try
                    {
                        // try to fill in the real username
                        var u = await Http.GetJson<RobloxUserSlim>(
                            $"https://users.roblox.com/v1/users/{numId}");
                        _currentUsername = u.Name;
                    }
                    catch { /* leave the "User {id}" fallback */ }
                }
                else
                {
                    var body    = JsonSerializer.Serialize(new { usernames = new[] { input }, excludeBannedUsers = false });
                    var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
                    var resp    = await App.HttpClient.PostAsync("https://users.roblox.com/v1/usernames/users", content);
                    resp.EnsureSuccessStatusCode();

                    var result = JsonSerializer.Deserialize<RobloxUsernameSearchResponse>(
                        await resp.Content.ReadAsStringAsync())!;

                    var found = result.Data.FirstOrDefault();
                    if (found is null)
                    {
                        ShowError($"No Roblox user found for \"{input}\".");
                        return;
                    }

                    _currentUserId   = found.Id;
                    _currentUsername = found.Name;
                }

                await FetchPage();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                ShowError("This user's inventory is set to private.");
            }
            catch (Exception ex)
            {
                ShowError($"Error: {ex.Message}");
            }
            finally
            {
                LoadingPanel.Visibility = Visibility.Collapsed;
                SearchButton.IsEnabled  = true;
            }
        }

        private async Task FetchPage()
        {
            string url = $"https://inventory.roblox.com/v1/users/{_currentUserId}" +
                         $"/assets/collectibles?sortOrder=Desc&limit=100";

            if (_nextCursor is not null)
                url += $"&cursor={Uri.EscapeDataString(_nextCursor)}";

            var response = await Http.GetJson<PagedResponse<CollectibleItem>>(url);
            var batch    = response.Data.ToList();
            _nextCursor  = response.NextPageCursor;

            // batch-fetch thumbnails
            Dictionary<long, string?> thumbMap = new();
            if (batch.Count > 0)
            {
                var ids = string.Join(",", batch.Select(i => i.AssetId).Distinct());
                try
                {
                    var tr = await Http.GetJson<ThumbnailBatchResponse>(
                        $"https://thumbnails.roblox.com/v1/assets?assetIds={ids}&size=110x110&format=Png");
                    thumbMap = tr.Data.ToDictionary(t => t.TargetId, t => t.ImageUrl);
                }
                catch { /* thumbnails are best-effort */ }
            }

            foreach (var item in batch)
            {
                thumbMap.TryGetValue(item.AssetId, out var thumb);
                _items.Add(new InventoryDisplayItem
                {
                    AssetId      = item.AssetId,
                    Name         = item.Name,
                    RAP          = item.RecentAveragePrice,
                    SerialNumber = item.SerialNumber,
                    ThumbnailUrl = thumb
                });
            }

            // update summary
            long totalRap = _items.Sum(i => i.RAP);
            SummaryUsernameText.Text = _currentUsername;
            SummaryItemsText.Text    = $"{_items.Count} collectible{(_items.Count == 1 ? "" : "s")}";
            SummaryRAPText.Text      = $"R$ {totalRap:N0}";

            SummaryBorder.Visibility  = Visibility.Visible;
            ItemsControl.Visibility   = _items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            LoadMoreButton.Visibility = _nextCursor is not null ? Visibility.Visible : Visibility.Collapsed;

            if (_items.Count == 0 && _nextCursor is null)
                ShowError("This user has no collectibles in their inventory.");
        }

        private void ShowError(string message)
        {
            ErrorText.Text            = message;
            ErrorPanel.Visibility     = Visibility.Visible;
            LoadingPanel.Visibility   = Visibility.Collapsed;
        }
    }

    // ── display model ─────────────────────────────────────────────────────────

    public class InventoryDisplayItem
    {
        public long    AssetId      { get; set; }
        public string  Name         { get; set; } = string.Empty;
        public long    RAP          { get; set; }
        public long?   SerialNumber { get; set; }
        public string? ThumbnailUrl { get; set; }

        public string  RAPDisplay    => RAP > 0 ? $"R$ {RAP:N0}" : "—";
        public string  SerialDisplay => SerialNumber.HasValue ? $"#{SerialNumber}" : string.Empty;
        public bool    HasSerial     => SerialNumber.HasValue;
    }

    // ── slim API models (only used here) ──────────────────────────────────────

    internal class RobloxUserSlim
    {
        [JsonPropertyName("id")]   public long   Id   { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    }

    internal class RobloxUsernameEntry
    {
        [JsonPropertyName("id")]   public long   Id   { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    }

    internal class RobloxUsernameSearchResponse
    {
        [JsonPropertyName("data")]
        public List<RobloxUsernameEntry> Data { get; set; } = new();
    }
}
