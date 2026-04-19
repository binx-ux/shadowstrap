using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using Shadowstrap.Enums;
using Shadowstrap.Enums.FlagPresets;

namespace Shadowstrap.UI.ViewModels.Settings
{
    public class FastFlagsViewModel : NotifyPropertyChangedViewModel
    {
        private Dictionary<string, object>? _preResetFlags;

        public event EventHandler? RequestPageReloadEvent;

        public event EventHandler? OpenFlagEditorEvent;

        private void OpenFastFlagEditor() => OpenFlagEditorEvent?.Invoke(this, EventArgs.Empty);

        public ICommand OpenFastFlagEditorCommand => new RelayCommand(OpenFastFlagEditor);

        public ICommand ApplyDefaultPresetCommand => new RelayCommand(() => ApplyPerformancePreset(PerformancePreset.Default));
        public ICommand ApplyBalancedPresetCommand => new RelayCommand(() => ApplyPerformancePreset(PerformancePreset.Balanced));
        public ICommand ApplyMaxPerformancePresetCommand => new RelayCommand(() => ApplyPerformancePreset(PerformancePreset.MaxPerformance));

        private void ApplyPerformancePreset(PerformancePreset preset)
        {
            App.FastFlags.ApplyPerformancePreset(preset);
            App.FastFlags.Save();

            // persist which preset is active so the cards can highlight it
            App.Settings.Prop.LastPerformancePreset = preset;
            App.Settings.Save();

            RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
        }

        // returns which preset card should appear highlighted
        public PerformancePreset CurrentPreset => App.Settings.Prop.LastPerformancePreset;

        public Visibility CanShowFastFlagEditor => App.IsStudioInstalled ? Visibility.Visible : Visibility.Collapsed;

        public bool UseFastFlagManager
        {
            get => App.Settings.Prop.UseFastFlagManager;
            set => App.Settings.Prop.UseFastFlagManager = value;
        }

        public IReadOnlyDictionary<MSAAMode, string?> MSAALevels => FastFlagManager.MSAAModes;

        public MSAAMode SelectedMSAALevel
        {
            get => MSAALevels.FirstOrDefault(x => x.Value == App.FastFlags.GetPreset("Rendering.MSAA")).Key;
            set { App.FastFlags.SetPreset("Rendering.MSAA", MSAALevels[value]); App.FastFlags.Save(); }
        }

        public IReadOnlyDictionary<RenderingMode, string> RenderingModes => FastFlagManager.RenderingModes;

        public RenderingMode SelectedRenderingMode
        {
            get => App.FastFlags.GetPresetEnum(RenderingModes, "Rendering.Mode", "True");
            set { App.FastFlags.SetPresetEnum("Rendering.Mode", RenderingModes[value], "True"); App.FastFlags.Save(); }
        }

        public bool FixDisplayScaling
        {
            get => App.FastFlags.GetPreset("Rendering.DisableScaling") == "True";
            set { App.FastFlags.SetPreset("Rendering.DisableScaling", value ? "True" : null); App.FastFlags.Save(); }
        }

        public IReadOnlyDictionary<TextureQuality, string?> TextureQualities => FastFlagManager.TextureQualityLevels;

        public TextureQuality SelectedTextureQuality
        {
            get => TextureQualities.Where(x => x.Value == App.FastFlags.GetPreset("Rendering.TextureQuality.Level")).FirstOrDefault().Key;
            set
            {
                if (value == TextureQuality.Default)
                    App.FastFlags.SetPreset("Rendering.TextureQuality", null);
                else
                {
                    App.FastFlags.SetPreset("Rendering.TextureQuality.OverrideEnabled", "True");
                    App.FastFlags.SetPreset("Rendering.TextureQuality.Level", TextureQualities[value]);
                }
                App.FastFlags.Save();
            }
        }
        public bool FPSCounterEnabled
        {
            get => App.FastFlags.GetPreset("Rendering.FPSCounter") == "True";
            set { App.FastFlags.SetPreset("Rendering.FPSCounter", value ? "True" : null); App.FastFlags.Save(); }
        }

        public bool ResetConfiguration
        {
            get => _preResetFlags is not null;

            set
            {
                if (value)
                {
                    _preResetFlags = new(App.FastFlags.Prop);
                    App.FastFlags.Prop.Clear();
                }
                else
                {
                    App.FastFlags.Prop = _preResetFlags!;
                    _preResetFlags = null;
                }

                RequestPageReloadEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
