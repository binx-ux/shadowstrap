<div align="center">

<img src="https://raw.githubusercontent.com/binx-ux/shadowstrap/main/Images/Shadowstrap-full-dark.png#gh-dark-mode-only" width="420" alt="Shadowstrap" />
<img src="https://raw.githubusercontent.com/binx-ux/shadowstrap/main/Images/Shadowstrap-full-light.png#gh-light-mode-only" width="420" alt="Shadowstrap" />

### A sleek, feature-rich Roblox bootstrapper replacement

[![License][badge-license]][url-license]
[![Release][badge-release]][url-releases]
[![Downloads][badge-downloads]][url-releases]
[![Discord][badge-discord]][url-discord]
[![Stars][badge-stars]][url-repo]

**[Download](https://github.com/binx-ux/shadowstrap/releases/latest)** · **[Website](https://binx-ux.github.io/shadowstrap)** · **[Wiki](https://github.com/binx-ux/shadowstrap/wiki)** · **[Discord](https://discord.gg/nKjV3mGq6R)**

</div>

---

> [!CAUTION]
> The only official places to download Shadowstrap are this GitHub repository and the [official website](https://binx-ux.github.io/shadowstrap). Any other source is not affiliated with us.

> [!NOTE]
> Shadowstrap is Windows-only. You will need the [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) if it is not already installed — you will be prompted automatically if it's missing.

---

## What is Shadowstrap?

Shadowstrap is a free, open-source replacement for the standard Roblox bootstrapper. It replaces the default Roblox launcher with a fully-featured settings menu, performance tools, integrations, and mod support — all in one sleek purple-themed package.

It does **not** modify the Roblox game client, inject code, or do anything that would put your account at risk.

---

## Features

### Performance
| Feature | Description |
|---|---|
| **Performance Presets** | One-click FastFlag bundles — Default, Balanced, Max Performance |
| **FPS Counter** | In-game overlay via `FFlagDebugDisplayFPS` |
| **Process Priority Boost** | Set Roblox CPU priority to Above Normal, High, or Realtime |
| **RAM Cleaner** | Trim process working sets before launch to free physical RAM |
| **Multi-Instance** | Run multiple Roblox windows simultaneously |

### Launch & Behaviour
| Feature | Description |
|---|---|
| **Auto-Rejoin** | Automatically relaunches Roblox 5 s after it exits |
| **Hone.gg Integration** | Auto-starts Hone.gg network optimizer before each launch |
| **Custom Integrations** | Launch any external app alongside Roblox |
| **Background Updates** | Keep Roblox up to date silently in the background |

### Mods & Appearance
| Feature | Description |
|---|---|
| **Custom Death Sound** | Replace the Roblox death sound with your own audio file |
| **Custom Cursor** | 2006 and 2013 era cursors built-in, or drop in your own |
| **Custom Font** | Replace the in-game UI font with any `.ttf`, `.otf`, or `.ttc` |
| **Old Character Sounds** | Restore the classic pre-2022 jump, land, and walk sounds |
| **Old Avatar Background** | Restore the classic avatar editor background |

### Integrations
| Feature | Description |
|---|---|
| **Discord Rich Presence** | Show your current game, server region, and playtime on Discord |
| **Activity Tracking** | Track which games you play and for how long |
| **Server Location** | See the geographic location of your current server |
| **Fast Flag Editor** | Directly edit any Roblox FastFlag with a built-in JSON editor |

---

## Installation

1. Download the latest `Shadowstrap.exe` from the [Releases page](https://github.com/binx-ux/shadowstrap/releases/latest)
2. Run it — Shadowstrap will install itself and register as the Roblox protocol handler
3. Launch Roblox from the website or Start Menu as normal

> Windows SmartScreen may show a warning the first time. Click **More info → Run anyway**. This is expected for new/unsigned software and does not indicate malware.

---

## FAQ

**Q: Is this malware?**
A: No. The full source code is publicly viewable here. Downloads are only served from this repository.

**Q: Will I get banned?**
A: No. Shadowstrap does not interact with the Roblox client the way exploits do. It only replaces the bootstrapper/launcher. [Read more.](https://github.com/binx-ux/shadowstrap/wiki/ban-safety)

**Q: Does it work with Roblox Studio?**
A: Studio launching is supported. Some player-specific features (Discord RPC, activity tracking) only apply to the player client.

**Q: How do I uninstall?**
A: Open Shadowstrap → Settings → Shadowstrap tab → Uninstall. This will restore the default Roblox bootstrapper.

---

## Building from Source

```bash
git clone --recurse-submodules https://github.com/binx-ux/shadowstrap
cd shadowstrap
dotnet build Shadowstrap/Shadowstrap.csproj -c Release
```

To publish a single-file executable:

```bash
dotnet publish Shadowstrap/Shadowstrap.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o ./publish
```

Requires [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

---

## Contributing

Pull requests are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) before opening one. For bugs and feature requests, use the [issue tracker](https://github.com/binx-ux/shadowstrap/issues).

---

## License

Shadowstrap is licensed under the [MIT License](LICENSE).

---

[badge-license]:   https://img.shields.io/github/license/binx-ux/shadowstrap?color=8b5cf6&style=for-the-badge
[badge-release]:   https://img.shields.io/github/v/release/binx-ux/shadowstrap?color=8b5cf6&style=for-the-badge&label=release
[badge-downloads]: https://img.shields.io/github/downloads/binx-ux/shadowstrap/total?color=8b5cf6&style=for-the-badge&label=downloads
[badge-discord]:   https://img.shields.io/discord/1099468797410283540?color=8b5cf6&style=for-the-badge&logo=discord&logoColor=white&label=discord
[badge-stars]:     https://img.shields.io/github/stars/binx-ux/shadowstrap?color=8b5cf6&style=for-the-badge&label=stars

[url-license]:  https://github.com/binx-ux/shadowstrap/blob/main/LICENSE
[url-releases]: https://github.com/binx-ux/shadowstrap/releases
[url-discord]:  https://discord.gg/nKjV3mGq6R
[url-repo]:     https://github.com/binx-ux/shadowstrap
