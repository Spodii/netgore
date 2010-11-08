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
        readonly IEnumerable<string> _columns;
        readonly string _customType;
        readonly IEnumerable<string> _tables;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeMapping"/> class.
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="customType">Type of the custom.</param>
        public CustomTypeMapping(IEnumerable<string> tables, IEnumerable<string> columns, string customType)
        {
            _tables = tables;
            _columns = columns.ToCompact();
            _customType = customType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeMapping"/> class.
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="customType">Type of the custom.</param>
        /// <param name="formatter">The formatter.</param>
        public CustomTypeMapping(IEnumerable<string> tables, IEnumerable<string> columns, Type customType, CodeFormatter formatter)
            : this(tables, columns, formatter.GetTypeString(customType))
        {
        }

        /// <summary>
        /// Gets the columns to use this mapping on.
        /// </summary>
        public IEnumerable<string> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Gets the custom type to expose.
        /// </summary>
        public string CustomType
        {
            get { return _customType; }
        }

        /// <summary>
        /// Gets the tables to use this mapping on.
        /// </summary>
        public IEnumerable<string> Tables
        {
            get { return _tables; }
        }
    }
}