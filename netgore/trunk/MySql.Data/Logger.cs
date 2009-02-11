using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Provides methods to output messages to our log
    /// </summary>
    class Logger
    {
        // private ctor
        Logger()
        {
        }

        public static void LogCommand(DBCmd cmd, string text)
        {
            string msg = String.Format("Executing command {0} with text ='{1}'", cmd, text);
            WriteLine(msg);
        }

        public static void LogException(Exception ex)
        {
            string msg = String.Format("EXCEPTION: " + ex.Message);
            WriteLine(msg);
        }

        public static void LogInformation(string msg)
        {
            Trace.WriteLine(msg);
        }

        public static void LogWarning(string s)
        {
            WriteLine("WARNING:" + s);
        }

        public static void Write(string s)
        {
            Trace.Write(s);
        }

        public static void WriteLine(string s)
        {
            Trace.WriteLine(String.Format("[{0}] - {1}", DateTime.Now, s));
        }
    }
}