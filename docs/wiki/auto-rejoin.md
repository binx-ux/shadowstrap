# Auto-Rejoin

Auto-Rejoin automatically relaunches Roblox after it exits, dropping you back into the same server if it's still running.

## Enabling It

Go to **Settings → Behaviour → Auto-Rejoin** and toggle it on.

> **Note:** Activity Tracking must also be enabled (it's on by default). Shadowstrap uses the activity log to pass the session URL to the Watcher process, which handles the relaunch.

## How It Works

1. When Roblox exits (for any reason — crash, disconnect, or closing the window), the Shadowstrap Watcher process detects the process has ended.
2. If Auto-Rejoin is enabled, the Watcher waits **5 seconds**.
3. It then re-fires the original `roblox://` URL via Windows shell execute.
4. Shadowstrap handles the relaunch the same as any normal launch.

If the original server is still running, you'll be placed back into it. If it has shut down, Roblox will matchmake you into another server for the same place.

## Things to Know

- The 5-second delay gives you a window to close Shadowstrap or kill the Watcher if you don't want to rejoin.
- Auto-Rejoin triggers regardless of *why* Roblox exited — network disconnect, crash, or intentional close. If you close Roblox on purpose and don't want to rejoin, you can disable the toggle.
- It does not loop indefinitely if rejoin fails — if the new Roblox session exits, it will attempt to rejoin again.
