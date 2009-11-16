using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

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

            static readonly Dictionary<Process, PerformanceCounter> _procCounters = new Dictionary<Process, PerformanceCounter>();

            /// <summary>
            /// Gets the current global CPU usage percent. This represents the percent of time the CPU is
            /// busy with any process in the system.
            /// </summary>
            public static float Usage
            {
                get { return _cpu.NextValue(); }
            }

            static readonly ReaderWriterLockSlim _procLock = new ReaderWriterLockSlim();

            /// <summary>
            /// Gets the current process CPU usage percent.
            /// </summary>
            public static float ProcessUsage
            {
                get
                {
                    PerformanceCounter counter;
                    Process process = Process.GetCurrentProcess();

                    _procLock.EnterUpgradeableReadLock();
                    {
                        if (!_procCounters.TryGetValue(process, out counter))
                        {
                            counter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
                            _procLock.EnterWriteLock();
                            {
                                _procCounters.Add(process, counter);
                            }
                            _procLock.ExitWriteLock();
                        }
                    }
                    _procLock.ExitUpgradeableReadLock();

                    return counter.NextValue();
                }
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
            public static int ProcessUsageMB
            {
                get
                {
                    return (int)(ProcessUsageBytes / 1024 / 1024);
                }
            }

            /// <summary>
            /// Gets the amount of system memory in use by this process in bytes.
            /// </summary>
            public static long ProcessUsageBytes
            {
                get
                {
                    return Process.GetCurrentProcess().PrivateMemorySize64;
                }
            }
        }
    }
}