# NIX (Shadowstrap) — Archived

> **This project is discontinued.** No further updates will be released.

Shadowstrap was a proof of concept — a test of what a Roblox bootstrapper could actually be. That question has been answered.

The final release is **v2.0.9**. The source code remains public for reference.

---

## Final binary

**[→ Download v2.0.9](https://github.com/binx-ux/shadowstrap/releases/tag/v2.0.9)**

If you have NIX installed, open it and click **Uninstall Now** in the dialog that appears, or go to **Settings → Shadowstrap → Uninstall** to restore the default Roblox launcher.

---

## Building from source

```bash
git clone --recurse-submodules https://github.com/binx-ux/shadowstrap
cd shadowstrap
dotnet publish Shadowstrap/Shadowstrap.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o ./publish
```

Requires [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

---

*Sunset Apr 2026. Not affiliated with Roblox Corporation.*
