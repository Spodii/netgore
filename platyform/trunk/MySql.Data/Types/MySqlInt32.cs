using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlInt32 : IMySqlValue
    {
        readonly bool is24Bit;
        readonly bool isNull;
        readonly int mValue;

        public int Value
        {
            get { return mValue; }
        }

        public MySqlInt32(MySqlDbType type, bool isNull) : this(type)
        {
            this.isNull = isNull;
        }

        public MySqlInt32(MySqlDbType type, int val) : this(type)
        {
            isNull = false;
            mValue = val;
        }

        MySqlInt32(MySqlDbType type)
        {
            is24Bit = type == MySqlDbType.Int24 ? true : false;
            isNull = true;
            mValue = 0;
        }

        internal static void SetDSInfo(DataTable dsTable)
        {
            var types = new string[] { "INT", "YEAR", "MEDIUMINT" };
            var dbtype = new MySqlDbType[] { MySqlDbType.Int32, MySqlDbType.Year, MySqlDbType.Int24 };

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
                row["DataType"] = "System.Int32";
                row["IsAutoincrementable"] = dbtype[x] == MySqlDbType.Year ? false : true;
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

        #region IMySqlValue Members

        public bool IsNull
        {
            get { return isNull; }
        }

        MySqlDbType IMySqlValue.MySqlDbType
        {
            get { return MySqlDbType.Int32; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.Int32; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(Int32); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return is24Bit ? "MEDIUMINT" : "INT"; }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            int v = Convert.ToInt32(val);
            if (binary)
                stream.Write(BitConverter.GetBytes(v));
            else
                stream.WriteStringNoNull(v.ToString());
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlInt32((this as IMySqlValue).MySqlDbType, true);

            if (length == -1)
                return new MySqlInt32((this as IMySqlValue).MySqlDbType, stream.ReadInteger(4));
            else
                return new MySqlInt32((this as IMySqlValue).MySqlDbType,
                                      Int32.Parse(stream.ReadString(length), CultureInfo.InvariantCulture));
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            stream.SkipBytes(4);
        }

        #endregion
    }
}