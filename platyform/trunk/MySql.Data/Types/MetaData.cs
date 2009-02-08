using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    class MetaData
    {
        public static bool IsNumericType(string typename)
        {
            string lowerType = typename.ToLower(CultureInfo.InvariantCulture);
            switch (lowerType)
            {
                case "int":
                case "integer":
                case "numeric":
                case "decimal":
                case "dec":
                case "fixed":
                case "tinyint":
                case "mediumint":
                case "bigint":
                case "real":
                case "double":
                case "float":
                case "serial":
                case "smallint":
                    return true;
            }
            return false;
        }

        public static MySqlDbType NameToType(string typeName, bool unsigned, bool realAsFloat, MySqlConnection connection)
        {
            switch (typeName.ToLower(CultureInfo.InvariantCulture))
            {
                case "char":
                    return MySqlDbType.String;
                case "varchar":
                    return MySqlDbType.VarChar;
                case "date":
                    return MySqlDbType.Date;
                case "datetime":
                    return MySqlDbType.DateTime;
                case "numeric":
                case "decimal":
                case "dec":
                case "fixed":
                    if (connection.driver.Version.isAtLeast(5, 0, 3))
                        return MySqlDbType.NewDecimal;
                    else
                        return MySqlDbType.Decimal;
                case "year":
                    return MySqlDbType.Year;
                case "time":
                    return MySqlDbType.Time;
                case "timestamp":
                    return MySqlDbType.Timestamp;
                case "set":
                    return MySqlDbType.Set;
                case "enum":
                    return MySqlDbType.Enum;
                case "bit":
                    return MySqlDbType.Bit;

                case "tinyint":
                    return unsigned ? MySqlDbType.UByte : MySqlDbType.Byte;
                case "bool":
                case "boolean":
                    return MySqlDbType.Byte;
                case "smallint":
                    return unsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16;
                case "mediumint":
                    return unsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24;
                case "int":
                case "integer":
                    return unsigned ? MySqlDbType.UInt32 : MySqlDbType.Int32;
                case "serial":
                    return MySqlDbType.UInt64;
                case "bigint":
                    return unsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64;
                case "float":
                    return MySqlDbType.Float;
                case "double":
                    return MySqlDbType.Double;
                case "real":
                    return realAsFloat ? MySqlDbType.Float : MySqlDbType.Double;
                case "text":
                    return MySqlDbType.Text;
                case "blob":
                    return MySqlDbType.Blob;
                case "longblob":
                    return MySqlDbType.LongBlob;
                case "longtext":
                    return MySqlDbType.LongText;
                case "mediumblob":
                    return MySqlDbType.MediumBlob;
                case "mediumtext":
                    return MySqlDbType.MediumText;
                case "tinyblob":
                    return MySqlDbType.TinyBlob;
                case "tinytext":
                    return MySqlDbType.TinyText;
                case "binary":
                    return MySqlDbType.Binary;
                case "varbinary":
                    return MySqlDbType.VarBinary;
            }
            throw new MySqlException("Unhandled type encountered");
        }

        public static bool SupportScale(string typename)
        {
            string lowerType = typename.ToLower(CultureInfo.InvariantCulture);
            switch (lowerType)
            {
                case "numeric":
                case "decimal":
                case "dec":
                case "real":
                    return true;
            }
            return false;
        }
    }
}