using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MySql.Data.Types
{
    interface IMySqlValue
    {
        DbType DbType { get; }
        bool IsNull { get; }
        MySqlDbType MySqlDbType { get; }
        string MySqlTypeName { get; }
        Type SystemType { get; }
        object Value { get; /*set;*/ }

        IMySqlValue ReadValue(MySqlStream stream, long length, bool isNull);

        void SkipValue(MySqlStream stream);

        void WriteValue(MySqlStream stream, bool binary, object value, int length);
    }
}