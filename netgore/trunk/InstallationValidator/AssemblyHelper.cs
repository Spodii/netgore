using System;
using System.IO;
using System.Reflection;

namespace InstallationValidator
{
    public static class AssemblyHelper
    {
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
    }
}