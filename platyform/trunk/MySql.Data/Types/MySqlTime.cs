using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlTimeSpan : IMySqlValue
    {
        bool isNull;
        TimeSpan mValue;

        public TimeSpan Value
        {
            get { return mValue; }
        }

        public MySqlTimeSpan(bool isNull)
        {
            this.isNull = isNull;
            mValue = TimeSpan.MinValue;
        }

        public MySqlTimeSpan(TimeSpan val)
        {
            isNull = false;
            mValue = val;
        }

        void ParseMySql(string s)
        {
            var parts = s.Split(':');
            int hours = Int32.Parse(parts[0]);
            int mins = Int32.Parse(parts[1]);
            int secs = Int32.Parse(parts[2]);
            if (hours < 0 || parts[0].StartsWith("-"))
            {
                mins *= -1;
                secs *= -1;
            }
            int days = hours / 24;
            hours = hours - (days * 24);
            mValue = new TimeSpan(days, hours, mins, secs, 0);
            isNull = false;
        }

        internal static void SetDSInfo(DataTable dsTable)
        {
            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            DataRow row = dsTable.NewRow();
            row["TypeName"] = "TIME";
            row["ProviderDbType"] = MySqlDbType.Time;
            row["ColumnSize"] = 0;
            row["CreateFormat"] = "TIME";
            row["CreateParameters"] = null;
            row["DataType"] = "System.TimeSpan";
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

        public override string ToString()
        {
            return String.Format("{0} {1:00}:{2:00}:{3:00}.{4}", mValue.Days, mValue.Hours, mValue.Minutes, mValue.Seconds,
                                 mValue.Milliseconds);
        }

        #region IMySqlValue Members

        public bool IsNull
        {
            get { return isNull; }
        }

        MySqlDbType IMySqlValue.MySqlDbType
        {
            get { return MySqlDbType.Time; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.Time; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(TimeSpan); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return "TIME"; }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            if (!(val is TimeSpan))
                throw new MySqlException("Only TimeSpan objects can be serialized by MySqlTimeSpan");

            TimeSpan ts = (TimeSpan)val;
            bool negative = ts.TotalMilliseconds < 0;
            ts = ts.Duration();

            if (binary)
            {
                stream.WriteByte(8);
                stream.WriteByte((byte)(negative ? 1 : 0));
                stream.WriteInteger(ts.Days, 4);
                stream.WriteByte((byte)ts.Hours);
                stream.WriteByte((byte)ts.Minutes);
                stream.WriteByte((byte)ts.Seconds);
            }
            else
            {
                String s = String.Format("'{0}{1} {2:00}:{3:00}:{4:00}.{5}'", negative ? "-" : "", ts.Days, ts.Hours, ts.Minutes,
                                         ts.Seconds, ts.Milliseconds);

                stream.WriteStringNoNull(s);
            }
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlTimeSpan(true);

            if (length >= 0)
            {
                string value = stream.ReadString(length);
                ParseMySql(value);
                return this;
            }

            long bufLength = stream.ReadByte();
            int negate = 0;
            if (bufLength > 0)
                negate = stream.ReadByte();

            isNull = false;
            if (bufLength == 0)
                isNull = true;
            else if (bufLength == 5)
                mValue = new TimeSpan(stream.ReadInteger(4), 0, 0, 0);
            else if (bufLength == 8)
                mValue = new TimeSpan(stream.ReadInteger(4), stream.ReadByte(), stream.ReadByte(), stream.ReadByte());
            else
            {
                mValue = new TimeSpan(stream.ReadInteger(4), stream.ReadByte(), stream.ReadByte(), stream.ReadByte(),
                                      stream.ReadInteger(4) / 1000000);
            }

            if (negate == 1)
                mValue = mValue.Negate();
            return this;
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            int len = stream.ReadByte();
            stream.SkipBytes(len);
        }

        #endregion
    }
}