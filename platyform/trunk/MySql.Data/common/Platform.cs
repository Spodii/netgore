using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MySql.Data.Common
{
    class Platform
    {
        /// <summary>
        /// By creating a private ctor, we keep the compiler from creating a default ctor
        /// </summary>
        Platform()
        {
        }

        public static bool IsWindows()
        {
            OperatingSystem os = Environment.OSVersion;
            switch (os.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    return true;
            }
            return false;
        }
    }
}