# Common Issues

## Roblox won't launch after installing Shadowstrap

1. Make sure you have the [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed.
2. Try running Shadowstrap as Administrator once to let it register properly.
3. Check `%LocalAppData%\Shadowstrap\Logs\` for error messages.

## "No releases or repo not found" on download badge

The download counter only appears once a GitHub Release exists. If you're running a pre-release build, this is expected.

## Discord Rich Presence isn't showing

- Make sure **Discord is open** before launching Roblox.
- Ensure **Activity Status** is enabled in Discord Settings → Activity Privacy.
- Ensure both **Activity Tracking** and **Discord Rich Presence** are toggled on in Shadowstrap.
- See [Discord Rich Presence](discord-rich-presence.md) for full troubleshooting.

## Multi-Instance isn't working

- Shadowstrap must be running as **Administrator** to open Roblox process handles for mutex manipulation.
- The feature targets `RobloxPlayerBeta.exe` — make sure at least one instance is running before launching a second.

## Mods aren't applying

- Mods take effect on the **next** Roblox launch, not the current one.
- Check that the file is placed at the correct path inside the Modifications folder.
- If using a custom font, ensure the file is a valid `.ttf`, `.otf`, or `.ttc` (Shadowstrap validates the header).

## Shadowstrap is flagged by antivirus

This is a false positive. Shadowstrap uses P/Invoke (native Windows API calls) for features like multi-instance and RAM cleaning, which some AV heuristics flag. The full source code is available for inspection. You can add an exception in your AV settings.

## How do I find my log file?

See [Finding Your Log File](log-file.md).

---

Still stuck? Ask in the [Discord server](https://discord.gg/nKjV3mGq6R) or [open an issue](https://github.com/binx-ux/shadowstrap/issues).
