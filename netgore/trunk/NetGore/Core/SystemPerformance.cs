using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore
{
    /// <summary>
    /// System performance and resource status information.
    /// </summary>
    public static class SystemPerformance
    {
        /// <summary>
        /// Provides information about the system's CPU usage and availability.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class CPU
        {
            static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            /// <summary>
            /// The maximum number of times the <see cref="PerformanceCounter"/> can fail before it stops
            /// attempting to be used.
            /// </summary>
            const int _maxFailures = 10;

            /// <summary>
            /// The CPU <see cref="PerformanceCounter"/>.
            /// </summary>
            static PerformanceCounter _cpu;

            /// <summary>
            /// The current count on the number of times acquiring the <see cref="PerformanceCounter"/>'s
            /// value has failed.
            /// </summary>
            static int _numFailures = 0;

            /// <summary>
            /// Initializes the <see cref="CPU"/> class.
            /// </summary>
            static CPU()
            {
                try
                {
                    _cpu = new PerformanceCounter
                    { CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total" };
                }
                catch (PlatformNotSupportedException ex)
                {
                    const string errmsg =
                        "Could not create PerformanceCounter for the Processor usage since this system does not support it. Exception: {0}";

                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Could not create PerformanceCounter for the Processor usage. Exception: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                }
            }

            /// <summary>
            /// Gets the current global CPU usage percent. This represents the percent of time the CPU is
            /// busy with any process in the system.
            /// </summary>
            public static float Usage
            {
                get
                {
                    if (_cpu != null)
                    {
                        const string errmsg = "Failed to acquire processor usage percent. Exception: {0}";

                        try
                        {
                            return _cpu.NextValue();
                        }
                        catch (Exception ex)
                        {
                            var willDispose = false;

                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            // For some exceptions, we will fail immediately instead of retrying
                            if (ex is PlatformNotSupportedException || ex is UnauthorizedAccessException ||
                                ex is InvalidOperationException)
                                willDispose = true;
                            else
                            {
                                // For general exceptions, we will retry a few times before disposing
                                if (++_numFailures >= _maxFailures)
                                    willDispose = true;
                            }

                            // Dispose if needed
                            if (willDispose)
                            {
                                try
                                {
                                    _cpu.Dispose();
                                }
                                catch (Exception exDispose)
                                {
                                    const string errmsgDispose = "Failed to dispose PerformanceCounter `{0}`: {1}";
                                    if (log.IsErrorEnabled)
                                        log.ErrorFormat(errmsgDispose, _cpu, exDispose);
                                    Debug.Fail(string.Format(errmsgDispose, _cpu, exDispose));
                                }

                                _cpu = null;
                            }
                        }
                    }

                    return 0;
                }
            }
        }

        /// <summary>
        /// Provides information about the system's memory usage and availability.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class Memory
        {
            static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            /// <summary>
            /// The maximum number of times the <see cref="PerformanceCounter"/> can fail before it stops
            /// attempting to be used.
            /// </summary>
            const int _maxFailures = 10;

            /// <summary>
            /// The current process.
            /// </summary>
            static Process _currentProcess;

            /// <summary>
            /// The memory usage <see cref="PerformanceCounter"/>.
            /// </summary>
            static PerformanceCounter _memory;

            /// <summary>
            /// The current count on the number of times acquiring the <see cref="PerformanceCounter"/>'s
            /// value has failed.
            /// </summary>
            static int _numFailures = 0;

            /// <summary>
            /// Initializes the <see cref="Memory"/> class.
            /// </summary>
            static Memory()
            {
                // Create the memory PerformanceCounter
                try
                {
                    _memory = new PerformanceCounter("Memory", "Available MBytes");
                }
                catch (PlatformNotSupportedException ex)
                {
                    const string errmsg =
                        "Could not create PerformanceCounter for the Memory usage since this system does not support it. Exception: {0}";

                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to create Memory performance counter: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);

                    _memory = null;
                }

                // Get the current process
                try
                {
                    _currentProcess = Process.GetCurrentProcess();
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to get the current process: {0}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, ex);
                    Debug.Fail(string.Format(errmsg, ex));

                    _currentProcess = null;
                }
            }

            /// <summary>
            /// Gets the amount of system memory available in megabytes, or 0 if the value could not be acquired.
            /// </summary>
            public static int AvailableMB
            {
                get
                {
                    if (_memory == null)
                        return 0;

                    const string errmsg = "Failed to acquire memory usage. Exception: {0}";

                    try
                    {
                        return (int)_memory.NextValue();
                    }
                    catch (Exception ex)
                    {
                        var willDispose = false;

                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, ex);

                        // For some exceptions, we will fail immediately instead of retrying
                        if (ex is PlatformNotSupportedException || ex is UnauthorizedAccessException ||
                            ex is InvalidOperationException)
                            willDispose = true;
                        else
                        {
                            // For general exceptions, we will retry a few times before disposing
                            if (++_numFailures >= _maxFailures)
                                willDispose = true;
                        }

                        // Dispose if needed
                        if (willDispose)
                        {
                            try
                            {
                                _memory.Dispose();
                            }
                            catch (Exception exDispose)
                            {
                                const string errmsgDispose = "Failed to dispose PerformanceCounter `{0}`: {1}";
                                if (log.IsErrorEnabled)
                                    log.ErrorFormat(errmsgDispose, _memory, exDispose);
                                Debug.Fail(string.Format(errmsgDispose, _memory, exDispose));
                            }

                            _memory = null;
                        }
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Gets the amount of system memory in use by this process in bytes, or 0 if the value could not be acquired.
            /// </summary>
            public static long ProcessUsageBytes
            {
                get
                {
                    if (_currentProcess == null)
                        return 0;

                    try
                    {
                        return _currentProcess.PrivateMemorySize64;
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to acquire the private memory size of the current process. Exception: {0}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, ex);

                        _currentProcess = null;
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Gets the amount of system memory in use by this process in bytes.
            /// </summary>
            public static int ProcessUsageMB
            {
                get { return (int)(ProcessUsageBytes / (1024f * 1024f)); }
            }
        }
    }
}