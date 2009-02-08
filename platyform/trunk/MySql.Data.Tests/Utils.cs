using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MySql.Data.MySqlClient.Tests
{
    /// <summary>
    /// Summary description for Utils.
    /// </summary>
    public class Utils
    {
        public static byte[] CreateBlob(int size)
        {
            var buf = new byte[size];

            Random r = new Random();
            r.NextBytes(buf);
            return buf;
        }
    }
}