using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Shadowstrap.AppData;
using Shadowstrap.Models.APIs.Roblox;
using Shadowstrap.Models.Entities;
using Shadowstrap.Utility;

namespace Shadowstrap.UI.ViewModels
{
    public class ServerBrowserViewModel : NotifyPropertyChangedViewModel
    {
        private string  _placeIdText  = string.Empty;
        private bool    _isLoading;
        private bool    _hideFull;
        private string  _errorMessage = string.Empty;
        private bool    _hasError;
        private string? _nextCursor;
        private long    _placeId;
        private UniverseDetails? _universeDetails;
        private int     _totalLoaded;

        public string PlaceIdText
        {
            get => _placeIdText;
            set { _placeIdText = value; OnPropertyChanged(nameof(PlaceIdText)); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); OnPropertyChanged(nameof(IsNotLoading)); }
        }
        public bool IsNotLoading => !_isLoading;

        public bool HideFull
        {
            get => _hideFull;
            set { _hideFull = value; OnPropertyChanged(nameof(HideFull)); ApplyFilter(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        public bool HasError
        {
            get => _hasError;
            set { _hasError = value; OnPropertyChanged(nameof(HasError)); }
        }

        public bool HasGameInfo => _universeDetails is not null;

        public string GameName     => _universeDetails?.Data.Name ?? string.Empty;
        public string GamePlaying  => _universeDetails is not null ? $"{_universeDetails.Data.Playing:N0} playing" : string.Empty;
        public string GameThumb    => _universeDetails?.Thumbnail.ImageUrl ?? string.Empty;

        public bool CanLoadMore => !string.IsNullOrEmpty(_nextCursor) && !_isLoading;

        public string ServerCount => _totalLoaded == 0 ? "No servers loaded" : $"{Servers.Count} of {_totalLoaded} servers";

        public ObservableCollection<ServerInstance> Servers { get; } = new();

        private readonly List<ServerInstance> _allServers = new();

        public ICommand BrowseCommand   => new AsyncRelayCommand(BrowseAsync);
        public ICommand LoadMoreCommand => new AsyncRelayCommand(LoadMoreAsync);
        public ICommand JoinCommand     => new RelayCommand<ServerInstance>(JoinServer);

        private async Task BrowseAsync()
        {
            if (!long.TryParse(_placeIdText.Trim(), out _placeId) || _placeId <= 0)
            {
                SetError("Enter a valid numeric Place ID.");
                return;
            }

            ClearError();
            IsLoading = true;
            _allServers.Clear();
            Servers.Clear();
            _nextCursor = null;
            _universeDetails = null;
            _totalLoaded = 0;
            NotifyGameInfo();

            try
            {
                // Fetch universe ID then game details
                var universeResp = await Http.GetJson<UniverseIdResponse>(
                    $"https://apis.roblox.com/universes/v1/places/{_placeId}/universe");

                await UniverseDetails.FetchSingle(universeResp.UniverseId);
                _universeDetails = UniverseDetails.LoadFromCache(universeResp.UniverseId);
                NotifyGameInfo();

                // Fetch first page of servers
                var page = await Http.GetJson<PagedResponse<ServerInstance>>(
                    $"https://games.roblox.com/v1/games/{_placeId}/servers/Public?limit=100");

                _nextCursor = page.NextPageCursor;
                foreach (var s in page.Data)
                    _allServers.Add(s);
            }
            catch (Exception ex)
            {
                SetError($"Failed to load: {ex.Message}");
            }
            finally
            {
                _totalLoaded = _allServers.Count;
                ApplyFilter();
                IsLoading = false;
                OnPropertyChanged(nameof(CanLoadMore));
                OnPropertyChanged(nameof(ServerCount));
            }
        }

        private async Task LoadMoreAsync()
        {
            if (string.IsNullOrEmpty(_nextCursor) || _placeId == 0) return;

            IsLoading = true;
            try
            {
                var url = $"https://games.roblox.com/v1/games/{_placeId}/servers/Public?limit=100&cursor={Uri.EscapeDataString(_nextCursor)}";
                var page = await Http.GetJson<PagedResponse<ServerInstance>>(url);

                _nextCursor = page.NextPageCursor;
                foreach (var s in page.Data)
                    _allServers.Add(s);
            }
            catch (Exception ex)
            {
                SetError($"Failed to load more: {ex.Message}");
            }
            finally
            {
                _totalLoaded = _allServers.Count;
                ApplyFilter();
                IsLoading = false;
                OnPropertyChanged(nameof(CanLoadMore));
                OnPropertyChanged(nameof(ServerCount));
            }
        }

        private void ApplyFilter()
        {
            Servers.Clear();
            foreach (var s in _allServers)
            {
                if (_hideFull && s.IsFull) continue;
                Servers.Add(s);
            }
            OnPropertyChanged(nameof(ServerCount));
        }

        private void JoinServer(ServerInstance? server)
        {
            if (server is null || _placeId == 0) return;

            string deeplink = $"roblox://experiences/start?placeId={_placeId}&gameInstanceId={server.Id}";

            try
            {
                string playerPath = new RobloxPlayerData().ExecutablePath;
                Process.Start(playerPath, deeplink);
            }
            catch
            {
                Process.Start(new ProcessStartInfo { FileName = deeplink, UseShellExecute = true });
            }
        }

        private void SetError(string msg) { ErrorMessage = msg; HasError = true; }
        private void ClearError()         { ErrorMessage = string.Empty; HasError = false; }

        private void NotifyGameInfo()
        {
            OnPropertyChanged(nameof(HasGameInfo));
            OnPropertyChanged(nameof(GameName));
            OnPropertyChanged(nameof(GamePlaying));
            OnPropertyChanged(nameof(GameThumb));
        }
    }
}
