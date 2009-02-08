using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using MySql.Data.Common;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    /// <summary>
    /// 
    /// </summary>
    public struct MySqlDateTime : IMySqlValue, IConvertible, IComparable
    {
        readonly bool isNull;
        readonly MySqlDbType type;
        int day, hour;
        int millisecond;
        int minute;
        int month;
        int second;
        int year;

        /// <summary>Returns the day portion of this datetime</summary>
        public int Day
        {
            get { return day; }
            set { day = value; }
        }

        /// <summary>Returns the hour portion of this datetime</summary>
        public int Hour
        {
            get { return hour; }
            set { hour = value; }
        }

        /// <summary>
        /// Indicates if this object contains a value that can be represented as a DateTime
        /// </summary>
        public bool IsValidDateTime
        {
            get { return year != 0 && month != 0 && day != 0; }
        }

        /// <summary>
        /// Retrieves the millisecond value of this object.
        /// </summary>
        public int Millisecond
        {
            get { return millisecond; }
            set { millisecond = value; }
        }

        /// <summary>Returns the minute portion of this datetime</summary>
        public int Minute
        {
            get { return minute; }
            set { minute = value; }
        }

        /// <summary>Returns the month portion of this datetime</summary>
        public int Month
        {
            get { return month; }
            set { month = value; }
        }

        /// <summary>Returns the second portion of this datetime</summary>
        public int Second
        {
            get { return second; }
            set { second = value; }
        }

        /// <summary>
        /// Retrieves the value of this <see cref="MySqlDateTime"/> as a DateTime object.
        /// </summary>
        public DateTime Value
        {
            get { return GetDateTime(); }
        }

        /// <summary>Returns the year portion of this datetime</summary>
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        /// <summary>
        /// Constructs a new <b>MySqlDateTime</b> object by setting the individual time properties to
        /// the given values.
        /// </summary>
        /// <param name="year">The year to use.</param>
        /// <param name="month">The month to use.</param>
        /// <param name="day">The day to use.</param>
        /// <param name="hour">The hour to use.</param>
        /// <param name="minute">The minute to use.</param>
        /// <param name="second">The second to use.</param>
        public MySqlDateTime(int year, int month, int day, int hour, int minute, int second)
            : this(MySqlDbType.DateTime, year, month, day, hour, minute, second)
        {
        }

        /// <summary>
        /// Constructs a new <b>MySqlDateTime</b> object by using values from the given <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> object to copy.</param>
        public MySqlDateTime(DateTime dt) : this(MySqlDbType.DateTime, dt)
        {
        }

        /// <summary>
        /// Constructs a new <b>MySqlDateTime</b> object by copying the current value of the given object.
        /// </summary>
        /// <param name="mdt">The <b>MySqlDateTime</b> object to copy.</param>
        public MySqlDateTime(MySqlDateTime mdt)
        {
            year = mdt.Year;
            month = mdt.Month;
            day = mdt.Day;
            hour = mdt.Hour;
            minute = mdt.Minute;
            second = mdt.Second;
            millisecond = 0;
            type = MySqlDbType.DateTime;
            isNull = false;
        }

        /// <summary>
        /// Enables the contruction of a <b>MySqlDateTime</b> object by parsing a string.
        /// </summary>
        /// <param name="s"></param>
        public MySqlDateTime(string s) : this(Parse(s))
        {
        }

        internal MySqlDateTime(MySqlDbType type, int year, int month, int day, int hour, int minute, int second)
        {
            isNull = false;
            this.type = type;
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
            millisecond = 0;
        }

        internal MySqlDateTime(MySqlDbType type, bool isNull) : this(type, 0, 0, 0, 0, 0, 0)
        {
            this.isNull = isNull;
        }

        internal MySqlDateTime(MySqlDbType type, DateTime val) : this(type, 0, 0, 0, 0, 0, 0)
        {
            isNull = false;
            year = val.Year;
            month = val.Month;
            day = val.Day;
            hour = val.Hour;
            minute = val.Minute;
            second = val.Second;
            millisecond = val.Millisecond;
        }

        static string FormatDateCustom(string format, int monthVal, int dayVal, int yearVal)
        {
            format = format.Replace("MM", "{0:00}");
            format = format.Replace("M", "{0}");
            format = format.Replace("dd", "{1:00}");
            format = format.Replace("d", "{1}");
            format = format.Replace("yyyy", "{2:0000}");
            format = format.Replace("yy", "{3:00}");
            format = format.Replace("y", "{4:0}");

            int year2digit = yearVal - ((yearVal / 1000) * 1000);
            year2digit -= ((year2digit / 100) * 100);
            int year1digit = year2digit - ((year2digit / 10) * 10);

            return String.Format(format, monthVal, dayVal, yearVal, year2digit, year1digit);
        }

        /// <summary>Returns this value as a DateTime</summary>
        public DateTime GetDateTime()
        {
            if (!IsValidDateTime)
                throw new MySqlConversionException("Unable to convert MySQL date/time value to System.DateTime");

            return new DateTime(year, month, day, hour, minute, second);
        }

        internal static MySqlDateTime Parse(string s)
        {
            MySqlDateTime dt = new MySqlDateTime();
            return dt.ParseMySql(s, true);
        }

        internal static MySqlDateTime Parse(string s, DBVersion version)
        {
            MySqlDateTime dt = new MySqlDateTime();
            return dt.ParseMySql(s, version.isAtLeast(4, 1, 0));
        }

        MySqlDateTime Parse40Timestamp(string s)
        {
            int pos = 0;
            year = month = day = 1;
            hour = minute = second = 0;

            if (s.Length == 14 || s.Length == 8)
            {
                year = int.Parse(s.Substring(pos, 4));
                pos += 4;
            }
            else
            {
                year = int.Parse(s.Substring(pos, 2));
                pos += 2;
                if (year >= 70)
                    year += 1900;
                else
                    year += 2000;
            }

            if (s.Length > 2)
            {
                month = int.Parse(s.Substring(pos, 2));
                pos += 2;
            }
            if (s.Length > 4)
            {
                day = int.Parse(s.Substring(pos, 2));
                pos += 2;
            }
            if (s.Length > 8)
            {
                hour = int.Parse(s.Substring(pos, 2));
                minute = int.Parse(s.Substring(pos + 2, 2));
                pos += 4;
            }
            if (s.Length > 10)
                second = int.Parse(s.Substring(pos, 2));

            return new MySqlDateTime(type, year, month, day, hour, minute, second);
        }

        MySqlDateTime ParseMySql(string s, bool is41)
        {
            if (type == MySqlDbType.Timestamp && !is41)
                return Parse40Timestamp(s);

            var parts = s.Split('-', ' ', ':', '/');

            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            int hour = 0, minute = 0, second = 0;
            if (parts.Length > 3)
            {
                hour = int.Parse(parts[3]);
                minute = int.Parse(parts[4]);
                second = int.Parse(parts[5]);
            }

            return new MySqlDateTime(type, year, month, day, hour, minute, second);
        }

        void SerializeText(MySqlStream stream, MySqlDateTime value)
        {
            string val = String.Empty;

            if (type == MySqlDbType.Timestamp && !stream.Version.isAtLeast(4, 1, 0))
            {
                val = String.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", value.Year, value.Month, value.Day, value.Hour,
                                    value.Minute, value.Second);
            }
            else
            {
                val = String.Format("{0:0000}-{1:00}-{2:00}", value.Year, value.Month, value.Day);
                if (type != MySqlDbType.Date)
                    val = String.Format("{0}  {1:00}:{2:00}:{3:00}", val, value.Hour, value.Minute, value.Second);
            }
            stream.WriteStringNoNull("'" + val + "'");
        }

        internal static void SetDSInfo(DataTable dsTable)
        {
            var types = new string[] { "DATE", "DATETIME", "TIMESTAMP" };
            var dbtype = new MySqlDbType[] { MySqlDbType.Date, MySqlDbType.DateTime, MySqlDbType.Timestamp };

            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            for (int x = 0; x < types.Length; x++)
            {
                DataRow row = dsTable.NewRow();
                row["TypeName"] = types[x];
                row["ProviderDbType"] = dbtype[x];
                row["ColumnSize"] = 0;
                row["CreateFormat"] = types[x];
                row["CreateParameters"] = null;
                row["DataType"] = "System.DateTime";
                row["IsAutoincrementable"] = false;
                row["IsBestMatch"] = true;
                row["IsCaseSensitive"] = false;
                row["IsFixedLength"] = true;
                row["IsFixedPrecisionScale"] = true;
                row["IsLong"] = false;
                row["IsNullable"] = true;
                row["IsSearchable"] = true;
                row["IsSearchableWithLike"] = false;
                row["IsUnsigned"] = false;
                row["MaximumScale"] = 0;
                row["MinimumScale"] = 0;
                row["IsConcurrencyType"] = DBNull.Value;
                row["IsLiteralsSupported"] = false;
                row["LiteralPrefix"] = null;
                row["LiteralSuffix"] = null;
                row["NativeDataType"] = null;
                dsTable.Rows.Add(row);
            }
        }

        /// <summary>Returns a MySQL specific string representation of this value</summary>
        public override string ToString()
        {
            if (IsValidDateTime)
            {
                DateTime d = new DateTime(year, month, day, hour, minute, second);
                return (type == MySqlDbType.Date) ? d.ToString("d") : d.ToString();
            }

            string dateString = FormatDateCustom(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern, month, day, year);
            if (type == MySqlDbType.Date)
                return dateString;

            DateTime dt = new DateTime(1, 2, 3, hour, minute, second);
            dateString = String.Format("{0} {1}", dateString, dt.ToLongTimeString());
            return dateString;
        }

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            MySqlDateTime otherDate = (MySqlDateTime)obj;

            if (Year < otherDate.Year)
                return -1;
            else if (Year > otherDate.Year)
                return 1;

            if (Month < otherDate.Month)
                return -1;
            else if (Month > otherDate.Month)
                return 1;

            if (Day < otherDate.Day)
                return -1;
            else if (Day > otherDate.Day)
                return 1;

            if (Hour < otherDate.Hour)
                return -1;
            else if (Hour > otherDate.Hour)
                return 1;

            if (Minute < otherDate.Minute)
                return -1;
            else if (Minute > otherDate.Minute)
                return 1;

            if (Second < otherDate.Second)
                return -1;
            else if (Second > otherDate.Second)
                return 1;

            if (Millisecond < otherDate.Millisecond)
                return -1;
            else if (Millisecond > otherDate.Millisecond)
                return 1;

            return 0;
        }

        #endregion

        #region IConvertible Members

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return 0;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return 0;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return 0;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return GetDateTime();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return 0;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return false;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return 0;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return 0;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return 0;
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return null;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return 0;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return '\0';
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return 0;
        }

        TypeCode IConvertible.GetTypeCode()
        {
            return new TypeCode();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return 0;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return null;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return 0;
        }

        #endregion

        #region IMySqlValue Members

        /// <summary>
        /// Returns true if this datetime object has a null value
        /// </summary>
        public bool IsNull
        {
            get { return isNull; }
        }

        MySqlDbType IMySqlValue.MySqlDbType
        {
            get { return type; }
        }

        DbType IMySqlValue.DbType
        {
            get
            {
                if (type == MySqlDbType.Date || type == MySqlDbType.Newdate)
                    return DbType.Date;
                return DbType.DateTime;
            }
        }

        object IMySqlValue.Value
        {
            get { return GetDateTime(); }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(DateTime); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get
            {
                switch (type)
                {
                    case MySqlDbType.Date:
                        return "DATE";
                    case MySqlDbType.Newdate:
                        return "NEWDATE";
                    case MySqlDbType.Timestamp:
                        return "TIMESTAMP";
                }
                return "DATETIME";
            }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object value, int length)
        {
            MySqlDateTime dtValue;

            if (value is DateTime)
                dtValue = new MySqlDateTime(type, (DateTime)value);
            else if (value is string)
                dtValue = new MySqlDateTime(type, DateTime.Parse((string)value, CultureInfo.CurrentCulture));
            else if (value is MySqlDateTime)
                dtValue = (MySqlDateTime)value;
            else
                throw new MySqlException("Unable to serialize date/time value.");

            if (!binary)
            {
                SerializeText(stream, dtValue);
                return;
            }

            if (type == MySqlDbType.Timestamp)
                stream.WriteByte(11);
            else
                stream.WriteByte(7);

            stream.WriteInteger(dtValue.Year, 2);
            stream.WriteByte((byte)dtValue.Month);
            stream.WriteByte((byte)dtValue.Day);
            if (type == MySqlDbType.Date)
            {
                stream.WriteByte(0);
                stream.WriteByte(0);
                stream.WriteByte(0);
            }
            else
            {
                stream.WriteByte((byte)dtValue.Hour);
                stream.WriteByte((byte)dtValue.Minute);
                stream.WriteByte((byte)dtValue.Second);
            }

            if (type == MySqlDbType.Timestamp)
                stream.WriteInteger(dtValue.Millisecond, 4);
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlDateTime(type, true);

            if (length >= 0)
            {
                string value = stream.ReadString(length);
                return ParseMySql(value, stream.Version.isAtLeast(4, 1, 0));
            }

            long bufLength = stream.ReadByte();
            int year = 0, month = 0, day = 0;
            int hour = 0, minute = 0, second = 0;

            if (bufLength >= 4)
            {
                year = stream.ReadInteger(2);
                month = stream.ReadByte();
                day = stream.ReadByte();
            }

            if (bufLength > 4)
            {
                hour = stream.ReadByte();
                minute = stream.ReadByte();
                second = stream.ReadByte();
            }

            if (bufLength > 7)
                stream.ReadInteger(4);

            return new MySqlDateTime(type, year, month, day, hour, minute, second);
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            long len = stream.ReadByte();
            stream.SkipBytes((int)len);
        }

        #endregion

        /// <summary></summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static explicit operator DateTime(MySqlDateTime val)
        {
            if (!val.IsValidDateTime)
                return DateTime.MinValue;
            return val.GetDateTime();
        }
    }
}