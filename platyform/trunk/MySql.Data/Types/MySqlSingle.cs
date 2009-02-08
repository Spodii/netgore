using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlSingle : IMySqlValue
    {
        readonly bool isNull;
        readonly float mValue;

        public float Value
        {
            get { return mValue; }
        }

        public MySqlSingle(bool isNull)
        {
            this.isNull = isNull;
            mValue = 0.0f;
        }

        public MySqlSingle(float val)
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
            row["TypeName"] = "FLOAT";
            row["ProviderDbType"] = MySqlDbType.Float;
            row["ColumnSize"] = 0;
            row["CreateFormat"] = "FLOAT";
            row["CreateParameters"] = null;
            row["DataType"] = "System.Single";
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

        #region IMySqlValue Members

        public bool IsNull
        {
            get { return isNull; }
        }

        MySqlDbType IMySqlValue.MySqlDbType
        {
            get { return MySqlDbType.Float; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.Single; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(float); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return "FLOAT"; }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            Single v = Convert.ToSingle(val);
            if (binary)
                stream.Write(BitConverter.GetBytes(v));
            else
                stream.WriteStringNoNull(v.ToString("R", CultureInfo.InvariantCulture));
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlSingle(true);

            if (length == -1)
            {
                var b = new byte[4];
                stream.Read(b, 0, 4);
                return new MySqlSingle(BitConverter.ToSingle(b, 0));
            }
            return new MySqlSingle(Single.Parse(stream.ReadString(length), CultureInfo.InvariantCulture));
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            stream.SkipBytes(4);
        }

        #endregion
    }
}