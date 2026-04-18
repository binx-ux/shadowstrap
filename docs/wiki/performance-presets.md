# Performance Presets

Performance Presets are one-click combinations of Roblox FastFlags that tune the renderer for different goals. Find them in **Settings → Fast Flags → Performance Presets**.

## Available Presets

### Default
Removes all performance-related FastFlags and restores Roblox's default rendering behaviour. Use this to undo a previous preset.

### Balanced
A middle ground — higher FPS cap, reduced shadow intensity, and disabled global wind. Good for most players.

| Flag | Value | Effect |
|------|-------|--------|
| `DFIntTaskSchedulerTargetFps` | `9999` | Removes the FPS cap |
| `FIntRenderShadowIntensity` | `1` | Reduces shadow brightness |
| `FFlagGlobalWindRendering` | `False` | Disables wind simulation |
| `FFlagGlobalWindActivated` | `False` | Disables wind activation |

### Max Performance
Strips out as much visual overhead as possible. Best for players on lower-end hardware or anyone chasing the highest possible FPS.

| Flag | Value | Effect |
|------|-------|--------|
| `DFIntTaskSchedulerTargetFps` | `9999` | Removes the FPS cap |
| `FIntRenderShadowIntensity` | `0` | Disables shadow rendering |
| `DFIntCSGLevelOfDetail*` | `0` | Reduces LOD switching distance |
| `DFIntCullFactorPixelThreshold*` | `2147483647` | Aggressively culls off-screen objects |
| `FFlagGlobalWindRendering` | `False` | Disables wind simulation |
| `FFlagGlobalWindActivated` | `False` | Disables wind activation |

## Notes

- Presets only affect the flags listed above. Your other FastFlags are not changed.
- Applying a preset takes effect on the **next** Roblox launch.
- You can always revert to stock behaviour by applying **Default**.
- For fine-grained control, use the [Fast Flag Editor](fast-flag-editor.md) directly.
