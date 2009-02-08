using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.Common
{
    /// <summary>
    /// Summary description for Version.
    /// </summary>
    struct DBVersion
    {
        readonly int build;
        readonly int major;
        readonly int minor;
        readonly string srcString;

        public int Build
        {
            get { return build; }
        }

        public int Major
        {
            get { return major; }
        }

        public int Minor
        {
            get { return minor; }
        }

        public DBVersion(string s, int major, int minor, int build)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            srcString = s;
        }

        public bool isAtLeast(int majorNum, int minorNum, int buildNum)
        {
            if (major > majorNum)
                return true;
            if (major == majorNum && minor > minorNum)
                return true;
            if (major == majorNum && minor == minorNum && build >= buildNum)
                return true;
            return false;
        }

        public static DBVersion Parse(string versionString)
        {
            int start = 0;
            int index = versionString.IndexOf('.', start);
            if (index == -1)
                throw new MySqlException(Resources.BadVersionFormat);
            string val = versionString.Substring(start, index - start).Trim();
            int major = Convert.ToInt32(val, NumberFormatInfo.InvariantInfo);

            start = index + 1;
            index = versionString.IndexOf('.', start);
            if (index == -1)
                throw new MySqlException(Resources.BadVersionFormat);
            val = versionString.Substring(start, index - start).Trim();
            int minor = Convert.ToInt32(val, NumberFormatInfo.InvariantInfo);

            start = index + 1;
            int i = start;
            while (i < versionString.Length && Char.IsDigit(versionString, i))
            {
                i++;
            }
            val = versionString.Substring(start, i - start).Trim();
            int build = Convert.ToInt32(val, NumberFormatInfo.InvariantInfo);

            return new DBVersion(versionString, major, minor, build);
        }

        public override string ToString()
        {
            return srcString;
        }
    }
}