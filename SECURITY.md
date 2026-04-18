# Security Policy

## Supported Versions

Only the latest release of Shadowstrap receives security fixes. Please update before reporting.

| Version | Supported |
|---------|-----------|
| Latest  | ✅ Yes |
| Older   | ❌ No  |

## Reporting a Vulnerability

**Do not open a public GitHub issue for security vulnerabilities.**

If you discover a security issue — especially anything that could affect users downloading or running Shadowstrap — please report it privately:

1. **GitHub:** Use [Security → Report a vulnerability](https://github.com/binx-ux/shadowstrap/security/advisories/new) (private disclosure)
2. **Discord:** DM a maintainer on the [Discord server](https://discord.gg/nKjV3mGq6R)

Please include:
- A description of the vulnerability
- Steps to reproduce it
- Potential impact
- Any suggested fix if you have one

You can expect an acknowledgement within **48 hours** and a resolution timeline within **7 days** for critical issues.

We will credit you in the release notes if you wish.

## Scope

Issues we consider in-scope:
- Malicious code execution via Shadowstrap's update or install flow
- Privilege escalation exploits in Shadowstrap's process handling
- Supply chain attacks (dependency tampering)
- Anything that could allow a third party to replace the official executable undetected

Issues we consider out-of-scope:
- Roblox client vulnerabilities (report those to Roblox directly)
- Social engineering attacks (e.g. fake download sites — we can't control those)
- Theoretical issues with no practical exploit path
