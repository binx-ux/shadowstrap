using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Shadowstrap.Helpers
{
    internal static class MultiInstanceHelper
    {
        private const string LOG_IDENT = "MultiInstanceHelper";

        [DllImport("ntdll.dll")]
        private static extern uint NtQuerySystemInformation(int infoClass, IntPtr info, int size, out int returnLength);

        [DllImport("ntdll.dll")]
        private static extern uint NtQueryObject(IntPtr handle, int infoClass, IntPtr info, int size, out int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint access, bool inherit, int pid);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DuplicateHandle(IntPtr srcProcess, IntPtr srcHandle, IntPtr dstProcess, out IntPtr dstHandle, uint access, bool inherit, uint options);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        private const uint PROCESS_DUP_HANDLE = 0x0040;
        private const uint DUPLICATE_CLOSE_SOURCE = 0x1;
        private const uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;
        private const uint STATUS_SUCCESS = 0;
        private const int SystemHandleInformation = 16;
        private const int ObjectNameInformation = 1;

        [StructLayout(LayoutKind.Sequential)]
        private struct SystemHandleEntry
        {
            public ushort UniqueProcessId;
            public ushort CreatorBackTraceIndex;
            public byte ObjectTypeIndex;
            public byte HandleAttributes;
            public ushort HandleValue;
            public IntPtr Object;
            public uint GrantedAccess;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct UnicodeString
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr Buffer;
        }

        public static void CloseRobloxMutexes()
        {
            var robloxProcs = Process.GetProcessesByName("RobloxPlayerBeta");
            if (robloxProcs.Length == 0)
            {
                App.Logger.WriteLine(LOG_IDENT, "No running Roblox processes found");
                return;
            }

            foreach (var proc in robloxProcs)
            {
                App.Logger.WriteLine(LOG_IDENT, $"Closing singleton mutex in PID {proc.Id}");
                try
                {
                    var t = new Thread(() => CloseMutexesInProcess(proc.Id)) { IsBackground = true };
                    t.Start();
                    t.Join(TimeSpan.FromSeconds(10));
                }
                catch (Exception ex)
                {
                    App.Logger.WriteException(LOG_IDENT, ex);
                }
                finally
                {
                    proc.Dispose();
                }
            }
        }

        private static void CloseMutexesInProcess(int targetPid)
        {
            IntPtr targetProcess = OpenProcess(PROCESS_DUP_HANDLE, false, targetPid);
            if (targetProcess == IntPtr.Zero)
            {
                App.Logger.WriteLine(LOG_IDENT, $"Failed to open PID {targetPid} — try running as administrator");
                return;
            }

            try
            {
                // Grow buffer until NtQuerySystemInformation succeeds
                int size = 0x20000;
                IntPtr buf = IntPtr.Zero;
                uint status;
                int returnLen;

                do
                {
                    if (buf != IntPtr.Zero) Marshal.FreeHGlobal(buf);
                    size *= 2;
                    buf = Marshal.AllocHGlobal(size);
                    status = NtQuerySystemInformation(SystemHandleInformation, buf, size, out returnLen);
                }
                while (status == STATUS_INFO_LENGTH_MISMATCH && size < 64 * 1024 * 1024);

                if (status != STATUS_SUCCESS)
                {
                    App.Logger.WriteLine(LOG_IDENT, $"NtQuerySystemInformation failed: 0x{status:X8}");
                    Marshal.FreeHGlobal(buf);
                    return;
                }

                int count = Marshal.ReadInt32(buf);
                int entrySize = Marshal.SizeOf<SystemHandleEntry>();
                IntPtr entriesBase = buf + 4; // ULONG count is 4 bytes, entries follow

                for (int i = 0; i < count; i++)
                {
                    var entry = Marshal.PtrToStructure<SystemHandleEntry>(entriesBase + i * entrySize);
                    if (entry.UniqueProcessId != targetPid)
                        continue;

                    if (!DuplicateHandle(targetProcess, (IntPtr)entry.HandleValue, GetCurrentProcess(), out IntPtr dup, 0, false, 0))
                        continue;

                    string? name = GetHandleNameSafe(dup);
                    CloseHandle(dup);

                    if (name != null && name.IndexOf("RobloxSingletonMutex", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        App.Logger.WriteLine(LOG_IDENT, $"Closing singleton mutex handle 0x{entry.HandleValue:X4} in PID {targetPid}");
                        DuplicateHandle(targetProcess, (IntPtr)entry.HandleValue, GetCurrentProcess(), out _, 0, false, DUPLICATE_CLOSE_SOURCE);
                    }
                }

                Marshal.FreeHGlobal(buf);
            }
            finally
            {
                CloseHandle(targetProcess);
            }
        }

        // Wraps NtQueryObject in a timed thread to avoid blocking on pipe/mailslot handles
        private static string? GetHandleNameSafe(IntPtr handle)
        {
            string? result = null;
            var t = new Thread(() => { result = GetHandleName(handle); }) { IsBackground = true };
            t.Start();
            t.Join(TimeSpan.FromMilliseconds(100));
            return result;
        }

        private static string? GetHandleName(IntPtr handle)
        {
            int size = 2048;
            IntPtr buf = Marshal.AllocHGlobal(size);
            try
            {
                uint status = NtQueryObject(handle, ObjectNameInformation, buf, size, out _);
                if (status != STATUS_SUCCESS) return null;

                var us = Marshal.PtrToStructure<UnicodeString>(buf);
                if (us.Length == 0 || us.Buffer == IntPtr.Zero) return null;

                return Marshal.PtrToStringUni(us.Buffer, us.Length / 2);
            }
            catch { return null; }
            finally { Marshal.FreeHGlobal(buf); }
        }
    }
}
