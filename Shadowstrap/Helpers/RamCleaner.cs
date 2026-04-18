using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Shadowstrap.Helpers
{
    internal static class RamCleaner
    {
        private const string LOG_IDENT = "RamCleaner";

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMin, IntPtr dwMax);

        public static void Clean()
        {
            App.Logger.WriteLine(LOG_IDENT, "Trimming process working sets before Roblox launch");

            long freed = 0;
            int cleaned = 0;

            foreach (var proc in Process.GetProcesses())
            {
                try
                {
                    if (proc.Id <= 4) { proc.Dispose(); continue; }

                    long before = proc.WorkingSet64;
                    // -1/-1 forces Windows to trim the working set to its minimum
                    SetProcessWorkingSetSize(proc.Handle, (IntPtr)(-1), (IntPtr)(-1));
                    proc.Refresh();
                    freed += Math.Max(0, before - proc.WorkingSet64);
                    cleaned++;
                }
                catch { /* skip inaccessible processes */ }
                finally { proc.Dispose(); }
            }

            long freedMb = freed / (1024 * 1024);
            App.Logger.WriteLine(LOG_IDENT, $"Done — trimmed {cleaned} processes, freed ~{freedMb} MB");
        }
    }
}
