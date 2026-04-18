# Ban Safety

**Short answer: Shadowstrap will not get you banned.**

## Why

Shadowstrap replaces the **bootstrapper** — the small program that downloads, updates, and launches Roblox. It does not:

- Inject code into the Roblox client process
- Hook or patch Roblox's memory
- Bypass or disable Byfron (Roblox's anti-cheat)
- Modify game scripts or asset data at runtime

The Roblox client (`RobloxPlayerBeta.exe`) runs exactly as Roblox ships it. Shadowstrap simply launches it with specific arguments and applies file-based modifications (sounds, cursors, fonts) that are loaded by the game the same way any asset would be.

## What Shadowstrap Actually Changes

- **FastFlags** — configuration values Roblox itself uses and exposes. Roblox explicitly allows players to have these on their own machines.
- **Content files** (cursors, sounds, fonts) — client-side asset replacements loaded from disk, no different from what Roblox loads normally.
- **Bootstrapper replacement** — Shadowstrap acts as the launcher; Roblox itself is untouched.

## Multi-Instance

The multi-instance feature closes a Windows mutex handle in a running Roblox process. This is a local OS-level operation that Roblox's anti-cheat does not monitor. Many users have run multiple instances for years without issue.

## Disclaimer

While we are confident Shadowstrap is safe, we cannot make absolute guarantees — Roblox may change their policies at any time. Use your own judgement. That said, there is no known case of a user being banned for using Shadowstrap or its predecessor.
