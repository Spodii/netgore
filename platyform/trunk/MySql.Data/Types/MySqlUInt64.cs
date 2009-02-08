using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlUInt64 : IMySqlValue
    {
        readonly bool isNull;
        readonly ulong mValue;

        public ulong Value
        {
            get { return mValue; }
        }

        public MySqlUInt64(bool isNull)
        {
            this.isNull = isNull;
            mValue = 0;
        }

        public MySqlUInt64(ulong val)
        {
            isNull = false;
            mValue = val;
        }

        internal static void SetDSInfo(DataTable dsTable)
        {
            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            DataRow row = dsTable.NewRow();
            row["TypeName"] = "BIGINT";
            row["ProviderDbType"] = MySqlDbType.UInt64;
            row["ColumnSize"] = 0;
            row["CreateFormat"] = "BIGINT UNSIGNED";
            row["CreateParameters"] = null;
            row["DataType"] = "System.UInt64";
            row["IsAutoincrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = true;
            row["MaximumScale"] = 0;
            row["MinimumScale"] = 0;
            row["IsConcurrencyType"] = DBNull.Value;
            row["IsLiteralsSupported"] = false;
            row["LiteralPrefix"] = null;
            row["LiteralSuffix"] = null;
            row["NativeDataType"] = null;
            dsTable.Rows.Add(row);
        }

        #region IMySqlValue Members

        public bool IsNull
        {
            get { return isNull; }
        }

        MySqlDbType IMySqlValue.MySqlDbType
        {
            get { return MySqlDbType.UInt64; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.UInt64; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(ulong); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return "BIGINT"; }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            ulong v = Convert.ToUInt64(val);
            if (binary)
                stream.Write(BitConverter.GetBytes(v));
            else
                stream.WriteStringNoNull(v.ToString());
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlUInt64(true);

            if (length == -1)
                return new MySqlUInt64(stream.ReadLong(8));
            else
                return new MySqlUInt64(UInt64.Parse(stream.ReadString(length)));
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            stream.SkipBytes(8);
        }

        #endregion
    }
}