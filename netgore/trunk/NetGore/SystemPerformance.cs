using System.Diagnostics;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// System performance and resource status information.
    /// </summary>
    public static class SystemPerformance
    {
        /// <summary>
        /// System CPU information.
        /// </summary>
        public static class CPU
        {
            static readonly PerformanceCounter _cpu = new PerformanceCounter
            { CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total" };

            /// <summary>
            /// Gets the current global CPU usage percent. This represents the percent of time the CPU is
            /// busy with any process in the system.
            /// </summary>
            public static float Usage
            {
                get { return _cpu.NextValue(); }
            }
        }

        /// <summary>
        /// System memory information.
        /// </summary>
        public static class Memory
        {
            static readonly PerformanceCounter _memory = new PerformanceCounter("Memory", "Available MBytes");

            /// <summary>
            /// Gets the amount of system memory available in megabytes.
            /// </summary>
            public static int AvailableMB
            {
                get { return (int)_memory.NextValue(); }
            }

            /// <summary>
            /// Gets the amount of system memory in use by this process in bytes.
            /// </summary>
            public static long ProcessUsageBytes
            {
                get { return Process.GetCurrentProcess().PrivateMemorySize64; }
            }

            /// <summary>
            /// Gets the amount of system memory in use by this process in bytes.
            /// </summary>
            public static int ProcessUsageMB
            {
                get { return (int)(ProcessUsageBytes / 1024 / 1024); }
            }
        }
    }
}