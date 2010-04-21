using System;
using System.Diagnostics;
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
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// System CPU information.
        /// </summary>
        public static class CPU
        {
            /// <summary>
            /// The maximum number of times the <see cref="PerformanceCounter"/> can fail before it stops
            /// attempting to be used.
            /// </summary>
            const int _maxFailures = 10;

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
                    Debug.Fail(string.Format(errmsg, ex));
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
                        catch (InvalidOperationException ex)
                        {
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            _cpu = null;
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            _cpu = null;
                        }
                        catch (PlatformNotSupportedException ex)
                        {
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            _cpu = null;
                        }
                        catch (Exception ex)
                        {
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            if (++_numFailures >= _maxFailures)
                                _cpu = null;
                        }
                    }

                    return 0;
                }
            }
        }

        /// <summary>
        /// System memory information.
        /// </summary>
        public static class Memory
        {
            /// <summary>
            /// The maximum number of times the <see cref="PerformanceCounter"/> can fail before it stops
            /// attempting to be used.
            /// </summary>
            const int _maxFailures = 10;

            /// <summary>
            /// The current process.
            /// </summary>
            static Process _currentProcess = Process.GetCurrentProcess();

            static PerformanceCounter _memory = new PerformanceCounter("Memory", "Available MBytes");

            /// <summary>
            /// The current count on the number of times acquiring the <see cref="PerformanceCounter"/>'s
            /// value has failed.
            /// </summary>
            static int _numFailures = 0;

            /// <summary>
            /// Gets the amount of system memory available in megabytes.
            /// </summary>
            public static int AvailableMB
            {
                get
                {
                    if (_memory != null)
                    {
                        const string errmsg = "Failed to acquire memory usage. Exception: {0}";

                        try
                        {
                            return (int)_memory.NextValue();
                        }
                        catch (InvalidOperationException ex)
                        {
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            _memory = null;
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            _memory = null;
                        }
                        catch (PlatformNotSupportedException ex)
                        {
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            _memory = null;
                        }
                        catch (Exception ex)
                        {
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            if (++_numFailures >= _maxFailures)
                                _memory = null;
                        }
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Gets the amount of system memory in use by this process in bytes.
            /// </summary>
            public static long ProcessUsageBytes
            {
                get
                {
                    if (_currentProcess != null)
                    {
                        try
                        {
                            return _currentProcess.PrivateMemorySize64;
                        }
                        catch (Exception ex)
                        {
                            const string errmsg =
                                "Failed to acquire the private memory size of the current process. Exception: {0}";
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, ex);

                            _currentProcess = null;
                        }
                    }

                    return 0;
                }
            }

            /// <summary>
            /// Gets the amount of system memory in use by this process in bytes.
            /// </summary>
            public static int ProcessUsageMB
            {
                get
                {
                    var pu = ProcessUsageBytes;
                    if (pu == 0)
                        return 0;

                    try
                    {
                        return (int)(ProcessUsageBytes / 1024f / 1024f);
                    }
                    catch (DivideByZeroException)
                    {
                        return 0;
                    }
                }
            }
        }
    }
}