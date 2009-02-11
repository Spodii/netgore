using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    struct MySqlString : IMySqlValue
    {
        readonly bool isNull;
        readonly string mValue;
        readonly MySqlDbType type;

        public string Value
        {
            get { return mValue; }
        }

        public MySqlString(MySqlDbType type, bool isNull)
        {
            this.type = type;
            this.isNull = isNull;
            mValue = String.Empty;
        }

        public MySqlString(MySqlDbType type, string val)
        {
            this.type = type;
            isNull = false;
            mValue = val;
        }

        internal static void SetDSInfo(DataTable dsTable)
        {
            var types = new string[] { "CHAR", "VARCHAR", "SET", "ENUM", "TINYTEXT", "TEXT", "MEDIUMTEXT", "LONGTEXT" };
            var dbtype = new MySqlDbType[]
                         {
                             MySqlDbType.String, MySqlDbType.VarChar, MySqlDbType.Set, MySqlDbType.Enum, MySqlDbType.TinyText,
                             MySqlDbType.Text, MySqlDbType.MediumText, MySqlDbType.LongText
                         };

            // we use name indexing because this method will only be called
            // when GetSchema is called for the DataSourceInformation 
            // collection and then it wil be cached.
            for (int x = 0; x < types.Length; x++)
            {
                DataRow row = dsTable.NewRow();
                row["TypeName"] = types[x];
                row["ProviderDbType"] = dbtype[x];
                row["ColumnSize"] = 0;
                row["CreateFormat"] = x < 2 ? types[x] + "({0})" : types[x];
                row["CreateParameters"] = x < 2 ? "size" : null;
                row["DataType"] = "System.String";
                row["IsAutoincrementable"] = false;
                row["IsBestMatch"] = true;
                row["IsCaseSensitive"] = false;
                row["IsFixedLength"] = false;
                row["IsFixedPrecisionScale"] = true;
                row["IsLong"] = false;
                row["IsNullable"] = true;
                row["IsSearchable"] = true;
                row["IsSearchableWithLike"] = true;
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
            get { return type; }
        }

        DbType IMySqlValue.DbType
        {
            get { return DbType.String; }
        }

        object IMySqlValue.Value
        {
            get { return mValue; }
        }

        Type IMySqlValue.SystemType
        {
            get { return typeof(string); }
        }

        string IMySqlValue.MySqlTypeName
        {
            get { return type == MySqlDbType.Set ? "SET" : type == MySqlDbType.Enum ? "ENUM" : "VARCHAR"; }
        }

        void IMySqlValue.WriteValue(MySqlStream stream, bool binary, object val, int length)
        {
            string v = val.ToString();
            if (length > 0)
            {
                length = Math.Min(length, v.Length);
                v = v.Substring(0, length);
            }

            if (binary)
                stream.WriteLenString(v);
            else
                stream.WriteStringNoNull("'" + MySqlHelper.EscapeString(v) + "'");
        }

        IMySqlValue IMySqlValue.ReadValue(MySqlStream stream, long length, bool nullVal)
        {
            if (nullVal)
                return new MySqlString(type, true);

            string s;
            if (length == -1)
                s = stream.ReadLenString();
            else
                s = stream.ReadString(length);
            MySqlString str = new MySqlString(type, s);
            return str;
        }

        void IMySqlValue.SkipValue(MySqlStream stream)
        {
            long len = stream.ReadFieldLength();
            stream.SkipBytes((int)len);
        }

        #endregion
    }
}