# Modding

Shadowstrap includes a built-in mod system that lets you replace Roblox's content files — sounds, cursors, fonts — without touching the installation directly. Changes are applied on each launch and restored cleanly if you remove the mod.

## How It Works

Shadowstrap maintains a **Modifications** folder at:

```
%LocalAppData%\Shadowstrap\Modifications\
```

Any file placed there that matches a path inside the Roblox version directory will be copied over at launch (MD5-checked, so unchanged files are skipped). If you remove a mod file, the original is restored from Roblox's cached packages on next launch.

## Built-in Presets

### Mouse Cursor
Found in **Settings → Mods → Presets**. Options:
- **Default** — Roblox's current cursor
- **2006** — The classic low-res arrow cursor from 2006
- **2013** — The slightly updated 2013 cursor

### Old Avatar Background
Restores the classic avatar editor background (`ExtraContent\places\Mobile.rbxl`).

### Old Character Sounds
Restores the pre-2022 jump, land, walk, and get-up sounds.

### Custom Font
Pick any `.ttf`, `.otf`, or `.ttc` file from your computer. Shadowstrap patches the font family manifests so the game uses your font for UI text.

### Custom Death Sound
Pick any `.ogg`, `.mp3`, or `.wav` file. It's copied to `content\sounds\ouch.ogg` and loaded by Roblox in place of the default death sound.

## Manual Mods

You can drop any file directly into the Modifications folder. The path structure mirrors Roblox's version directory:

```
Modifications/
└── content/
    └── sounds/
        └── my_custom_sound.ogg
```

Click **Open Mods Folder** in **Settings → Mods** to open it in Explorer.

## Notes

- Mods take effect on the **next** Roblox launch.
- Removing a preset or deleting the file from Modifications restores the original.
- File replacements are per-version — if Roblox updates and the file changes, your mod still takes precedence (unless you remove it).
