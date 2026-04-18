# Contributing to Shadowstrap

Thank you for wanting to contribute! Here's everything you need to know.

---

## Before You Start

- Check the [open issues](https://github.com/binx-ux/shadowstrap/issues) to see if your bug or feature is already tracked.
- For large changes, open a **discussion issue first** so we can align before you invest time writing code.
- Read the [Code of Conduct](CODE_OF_CONDUCT.md) — it applies to all contributions.

---

## Setting Up the Dev Environment

```bash
# Clone with submodules (WPF UI is a submodule)
git clone --recurse-submodules https://github.com/binx-ux/shadowstrap
cd shadowstrap

# Build
dotnet build Shadowstrap/Shadowstrap.csproj -c Debug

# Publish single-file exe (for testing full build)
dotnet publish Shadowstrap/Shadowstrap.csproj -c Release -r win-x64 \
  --self-contained false -p:PublishSingleFile=true -o ./publish
```

**Requirements:**
- Windows 10/11 (x64)
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Visual Studio 2022 or Rider (optional but recommended)

---

## Project Structure

```
Shadowstrap/
├── App.xaml / App.xaml.cs         — Application entry, global state, settings
├── Bootstrapper.cs                — Core launch logic (install, update, launch Roblox)
├── Watcher.cs                     — Post-launch monitoring (runs alongside Roblox)
├── FastFlagManager.cs             — FastFlag preset system
├── Enums/                         — All enum types
├── Helpers/                       — Utility classes (MultiInstanceHelper, RamCleaner…)
├── Integrations/                  — ActivityWatcher, Discord RPC
├── Models/                        — Data models, settings, SettingTasks
├── Resources/                     — Localisation strings
└── UI/
    ├── Elements/Settings/Pages/   — Each settings page (XAML)
    ├── ViewModels/Settings/       — ViewModel for each page (C#, MVVM)
    └── Style/                     — Resource dictionaries (colours, brushes)
```

---

## Code Style

- Follow the existing patterns — MVVM for UI, `App.Settings.Prop.*` for settings, `App.Logger.WriteLine` for logging.
- No unnecessary comments. Code should be self-explanatory through naming.
- No trailing whitespace, no unused usings.
- Settings go in `Models/Persistable/Settings.cs`. Always provide a sensible default.
- New FastFlag presets go in `FastFlagManager.cs` (`PresetFlags` dictionary).

---

## Adding a New Feature

**Settings-backed feature (typical):**
1. Add property to `Models/Persistable/Settings.cs`
2. Add ViewModel property to the relevant `UI/ViewModels/Settings/XxxViewModel.cs`
3. Add UI control to the relevant `UI/Elements/Settings/Pages/XxxPage.xaml`
4. Wire up logic in `Bootstrapper.cs` (if launch-time) or `Watcher.cs` (if post-launch)

**FastFlag preset:**
1. Add key → flag mapping to `FastFlagManager.PresetFlags`
2. Add ViewModel property using `App.FastFlags.GetPreset` / `SetPreset`
3. Add toggle or control to `FastFlagsPage.xaml`

---

## Pull Request Guidelines

- **One feature or fix per PR.** Split unrelated changes into separate PRs.
- **Write a clear description.** Explain what changed and why. Link the issue if there is one.
- **Test your changes.** At minimum, run a full build (`dotnet build`) and verify the feature works end-to-end.
- **Don't reformat unrelated code.** Keep diffs focused — only change what your PR is about.
- **Target `main`.** All PRs should be opened against the `main` branch.

---

## Reporting Bugs

Use the [bug report template](https://github.com/binx-ux/shadowstrap/issues/new?template=bug_report.yaml). Include:

- Steps to reproduce
- Expected vs. actual behaviour
- Shadowstrap version
- Windows version
- Roblox log file if relevant (`%LocalAppData%\Roblox\logs\`)

---

## Feature Requests

Use the [feature request template](https://github.com/binx-ux/shadowstrap/issues/new?template=feature_request.yaml). Explain:

- What you want
- Why it would be useful
- Any implementation ideas you have

---

## Questions?

Jump into the [Discord server](https://discord.gg/nKjV3mGq6R) — that's the fastest way to get help.
