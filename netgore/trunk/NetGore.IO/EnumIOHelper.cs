using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    public static class EnumIOHelper
    {
        public static string ToName<T>(T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToString();
        }

        public static T FromName<T>(string value)where T : struct, IComparable, IConvertible, IFormattable
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
