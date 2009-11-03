﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace InstallationValidator
{
    public static class AssemblyHelper
    {
        /// <summary>
        /// Signature string for the Xna assembly.
        /// </summary>
        public const string XnaAssembly =
            "Microsoft.Xna.Framework, Version=3.1.0.0, Culture=neutral, PublicKeyToken=6d5c3888ef60e27d, processorArchitecture=x86";

        /// <summary>
        /// Signature string for the MySql.Data assembly.
        /// </summary>
        public const string MySqlDataAssembly =
            "MySql.Data, Version=6.1.0.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d, processorArchitecture=MSIL";

        static string GetAssemblyName(KnownAssembly a)
        {
            switch (a)
            {
                case KnownAssembly.MySqlData:
                    return MySqlDataAssembly;
                case KnownAssembly.Xna:
                    return XnaAssembly;
                default:
                    throw new ArgumentOutOfRangeException("a");
            }
        }

        public static Assembly TryLoadAssembly(KnownAssembly a)
        {
            return TryLoadAssembly(GetAssemblyName(a));
        }

        public static Assembly TryLoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.ReflectionOnlyLoad(assemblyName);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static bool IsAssemblyInstalled(KnownAssembly a)
        {
            return IsAssemblyInstalled(GetAssemblyName(a));
        }

        public static bool IsAssemblyInstalled(string assemblyName)
        {
            try
            {
                var asm = Assembly.ReflectionOnlyLoad(assemblyName);
                return asm != null;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
    }

    public enum KnownAssembly
    {
        MySqlData,
        Xna,
    }
}
