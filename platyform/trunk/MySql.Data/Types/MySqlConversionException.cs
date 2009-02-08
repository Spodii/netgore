using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MySql.Data.Types
{
    /// <summary>
    /// Summary description for MySqlConversionException.
    /// </summary>
    [Serializable]
    public class MySqlConversionException : Exception
    {
        /// <summary>Ctor</summary>
        public MySqlConversionException(string msg) : base(msg)
        {
        }
    }
}