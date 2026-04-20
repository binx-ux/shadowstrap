# Community FastFlag Presets

This folder contains community-contributed FastFlag presets for NIX.

## How to contribute

1. Fork the repo
2. Open `community/flags.json`
3. Add your preset to the `presets` array following the schema below
4. Open a pull request — include a short description of what the flags do and which games/hardware you tested on

## Preset schema

```json
{
  "name": "My Preset",
  "description": "What this preset does and who it's for.",
  "author": "your-github-username",
  "tags": ["performance", "competitive"],
  "flags": {
    "DFIntTaskSchedulerTargetFps": "9999"
  }
}
```

### Tag list

| Tag | Use when |
|-----|----------|
| `performance` | General FPS / GPU improvements |
| `competitive` | Reduced input lag, cleaner hitboxes |
| `low-end` | Very aggressive cuts for old hardware |
| `visual` | Quality improvements (higher resolution, better shadows) |
| `latency` | Network / ping related |
| `game:<name>` | Specific game (e.g. `game:jailbreak`) |

## Rules

- Flags must start with a [valid NIX prefix](https://github.com/binx-ux/shadowstrap/wiki) (`FFlag`, `DFFlag`, `FInt`, `DFInt`, `FString`, `DFString`, `FLog`, `DFLog`, `SFFlag`)
- Do not include flags that require a specific Roblox Studio version or are account-locked
- No flags that violate Roblox's Terms of Service
- One PR per preset or small related group of presets
- PRs that just spam max-FPS flags already covered by built-in presets will be closed

## Using presets in NIX

In the app, go to **Game Profiles → Community Flag Presets → Browse** or paste a preset JSON into the **Import Community Profile** dialog.
