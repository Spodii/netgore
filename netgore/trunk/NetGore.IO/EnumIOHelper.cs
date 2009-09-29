using System;
using System.Linq;

namespace NetGore.IO
{
    public static class EnumIOHelper
    {
        public static T FromName<T>(string value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static T ReadEnum<T>(IValueReader reader, IEnumValueReader<T> enumReader, string name)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            if (reader.UseEnumNames)
                return reader.ReadEnumName<T>(name);
            else
                return reader.ReadEnumValue(enumReader, name);
        }

        public static string ToName<T>(T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToString();
        }

        public static void WriteEnum<T>(IValueWriter writer, IEnumValueWriter<T> enumWriter, string name, T value)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            if (writer.UseEnumNames)
                writer.WriteEnumName(name, value);
            else
                writer.WriteEnumValue(enumWriter, name, value);
        }
    }
}