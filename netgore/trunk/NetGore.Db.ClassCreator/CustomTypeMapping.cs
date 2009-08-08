using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Provides the mapping for the custom Type to use for a column or collection of columns.
    /// </summary>
    public class CustomTypeMapping
    {
        /// <summary>
        /// The columns to use this mapping on.
        /// </summary>
        public readonly IEnumerable<string> Columns;

        /// <summary>
        /// The custom type to expose.
        /// </summary>
        public readonly string CustomType;

        /// <summary>
        /// The tables to use this mapping on.
        /// </summary>
        public readonly IEnumerable<string> Tables;

        public CustomTypeMapping(IEnumerable<string> tables, IEnumerable<string> columns, string customType)
        {
            Tables = tables;
            Columns = columns.ToArray();
            CustomType = customType;
        }

        public CustomTypeMapping(IEnumerable<string> tables, IEnumerable<string> columns, Type customType, CodeFormatter formatter)
            : this(tables, columns, formatter.GetTypeString(customType))
        {
        }
    }
}