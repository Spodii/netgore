using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlInt16 : IMySqlValue
    {
        readonly bool isNull;
        readonly short mValue;

        public short Value
        {
            get { return mValue; }
        }

        public MySqlInt16(bool isNull)
        {
            this.isNull = isNull;
            mValue = 0;
        }

        public MySqlInt16(short val)
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
            row["TypeName"] = "SMALLINT";
            row["ProviderDbType"] = MySqlDbType.Int16;
            row["ColumnSize"] = 0;
            row["CreateFormat"] = "SMALLINT";
            row["CreateParameters"] = null;
            row["DataType"] = "System.Int16";
            row["IsAutoincrementable"] = true;
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

        #region IMySqlValue Members

        public bool IsNull
        {
            get { return isNull; }
        }

        MySqlDbType IMySqlValue.MySqlDbType
        {
            get { return MySqlDbType.Int16; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.Int16; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(short); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return "SMALLINT"; }
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
                return new MySqlInt16(true);

            if (length == -1)
                return new MySqlInt16((short)stream.ReadInteger(2));
            else
                return new MySqlInt16(Int16.Parse(stream.ReadString(length)));
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            stream.SkipBytes(2);
        }

        #endregion
    }
}