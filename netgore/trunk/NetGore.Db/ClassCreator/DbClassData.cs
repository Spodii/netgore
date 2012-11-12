using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Contains the data used for generating a class for a database table.
    /// </summary>
    public class DbClassData
    {
        readonly string _className;
        readonly IEnumerable<ColumnCollection> _columnCollections;
        readonly IEnumerable<DbColumnInfo> _columns;

        readonly IEnumerable<CustomTypeMapping> _customTypes;
        readonly Dictionary<Type, string> _dataReaderReadMethods;
        readonly string _extensionClassName;
        readonly IDictionary<DbColumnInfo, string> _externalTypes = new Dictionary<DbColumnInfo, string>();
        readonly CodeFormatter _formatter;
        readonly string _interfaceName;
        readonly IDictionary<DbColumnInfo, string> _parameterNames = new Dictionary<DbColumnInfo, string>();
        readonly IDictionary<DbColumnInfo, string> _privateNames = new Dictionary<DbColumnInfo, string>();
        readonly IDictionary<DbColumnInfo, string> _publicNames = new Dictionary<DbColumnInfo, string>();
        readonly string _tableName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbClassData"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="dataReaderReadMethods">The data reader read methods.</param>
        /// <param name="columnCollections">The column collections.</param>
        /// <param name="customTypes">The custom types.</param>
        public DbClassData(string tableName, IEnumerable<DbColumnInfo> columns, CodeFormatter formatter,
                           Dictionary<Type, string> dataReaderReadMethods, IEnumerable<ColumnCollection> columnCollections,
                           IEnumerable<CustomTypeMapping> customTypes)
        {
            const string tableNameWildcard = "*";

            _tableName = tableName;
            _columns = columns.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
            _formatter = formatter;
            _dataReaderReadMethods = dataReaderReadMethods;

            _className = formatter.GetClassName(tableName);
            _interfaceName = formatter.GetInterfaceName(tableName);
            _extensionClassName = ClassName + "DbExtensions";

            // Custom types filter
            _customTypes = customTypes.Where(x =>
                !x.Columns.IsEmpty() && (x.Tables.Contains(TableName, StringComparer.OrdinalIgnoreCase) || x.Tables.Contains(tableNameWildcard))).
                ToCompact();

            // Column collections filter
            _columnCollections = columnCollections.Where(x =>
                !x.Columns.IsEmpty() &&
                (x.Tables.Contains(TableName, StringComparer.OrdinalIgnoreCase) || x.Tables.Contains(tableNameWildcard))).
                OrderBy(x => x.CollectionPropertyName).ToCompact();

            // Populate the external types dictionary
            foreach (var column in columns)
            {
                var columnName = column.Name;
                string externalType;
                var customType = _customTypes.FirstOrDefault(x => x.Columns.Contains(columnName, StringComparer.OrdinalIgnoreCase));
                if (customType != null)
                    externalType = customType.CustomType;
                else
                    externalType = GetInternalType(column);

                _externalTypes.Add(column, externalType);
            }

            // Populate the naming dictionaries
            foreach (var column in columns)
            {
                _privateNames.Add(column, formatter.GetFieldName(column.Name, MemberVisibilityLevel.Private, GetInternalType(column)));
                _publicNames.Add(column, formatter.GetFieldName(column.Name, MemberVisibilityLevel.Public, GetExternalType(column)));
                _parameterNames.Add(column, formatter.GetParameterName(column.Name, column.Type));
            }
        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }

        /// <summary>
        /// Gets the column collections.
        /// </summary>
        public IEnumerable<ColumnCollection> ColumnCollections
        {
            get { return _columnCollections; }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        public IEnumerable<DbColumnInfo> Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Gets the name of the extension class.
        /// </summary>
        public string ExtensionClassName
        {
            get { return _extensionClassName; }
        }

        /// <summary>
        /// Gets the code formatter.
        /// </summary>
        public CodeFormatter Formatter
        {
            get { return _formatter; }
        }

        /// <summary>
        /// Gets the name of the interface.
        /// </summary>
        public string InterfaceName
        {
            get { return _interfaceName; }
        }

        /// <summary>
        /// Gets the name of the database table.
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }

        /// <summary>
        /// Ensures a <see cref="Type"/> is nullable.
        /// </summary>
        /// <param name="type">The full name of the <see cref="Type"/>.</param>
        /// <returns>The name of the <paramref name="type"/> with support of being nullable.</returns>
        public string EnsureIsNullable(string type)
        {
            // Do not try to make strings nullable
            if (type.Equals(typeof(string).Name, StringComparison.OrdinalIgnoreCase) ||
                type.Equals(typeof(string).FullName, StringComparison.OrdinalIgnoreCase))
                return type;

            // If not already nullable, make nullable
            if (!type.StartsWith("System.Nullable", StringComparison.OrdinalIgnoreCase) && !type.EndsWith("?"))
                return "System.Nullable" + Formatter.OpenGeneric + type + Formatter.CloseGeneric;

            // Already nullable
            return type;
        }

        /// <summary>
        /// Gets the <see cref="ColumnCollection"/> for a given <see cref="DbColumnInfo"/>, or null if the
        /// <see cref="DbColumnInfo"/> is not part of any <see cref="ColumnCollection"/> in this table.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the <see cref="ColumnCollection"/> for.</param>
        /// <returns>The <see cref="ColumnCollection"/> the <see cref="DbColumnInfo"/> is part of, or null if it
        /// is not part of a <see cref="ColumnCollection"/>.</returns>
        public ColumnCollection GetCollectionForColumn(DbColumnInfo dbColumn)
        {
            ColumnCollectionItem item;
            return GetCollectionForColumn(dbColumn, out item);
        }

        /// <summary>
        /// Gets the <see cref="ColumnCollection"/> for a given <see cref="DbColumnInfo"/>, or null if the
        /// <see cref="DbColumnInfo"/> is not part of any <see cref="ColumnCollection"/> in this table.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the <see cref="ColumnCollection"/> for.</param>
        /// <param name="item">The <see cref="ColumnCollectionItem"/> for the <paramref name="dbColumn"/> in the
        /// <see cref="ColumnCollection"/>.</param>
        /// <returns>The <see cref="ColumnCollection"/> the <see cref="DbColumnInfo"/> is part of, or null if it
        /// is not part of a <see cref="ColumnCollection"/>.</returns>
        /// <exception cref="ArgumentException">The <paramref name="dbColumn"/> matched more than one <see cref="ColumnCollection"/>.</exception>
        public ColumnCollection GetCollectionForColumn(DbColumnInfo dbColumn, out ColumnCollectionItem item)
        {
            foreach (var columnCollection in ColumnCollections)
            {
                var matches =
                    columnCollection.Columns.Where(x => x.ColumnName.Equals(dbColumn.Name, StringComparison.OrdinalIgnoreCase));

                var count = matches.Count();
                if (count == 1)
                {
                    item = matches.First();
                    return columnCollection;
                }
                else if (count > 1)
                {
                    const string errmsg = "DbColumnInfo for column `{0}` in table `{1}` matched more than one ColumnCollection!";
                    throw new ArgumentException(string.Format(errmsg, dbColumn.Name, TableName));
                }
            }

            item = default(ColumnCollectionItem);
            return null;
        }

        /// <summary>
        /// Gets the code to use for the accessor for a <see cref="DbColumnInfo"/>.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the value accessor for.</param>
        /// <returns>The code to use for the accessor for a <see cref="DbColumnInfo"/>.</returns>
        public string GetColumnValueAccessor(DbColumnInfo dbColumn)
        {
            ColumnCollectionItem item;
            var coll = GetCollectionForColumn(dbColumn, out item);

            if (coll == null)
            {
                // Not part of a collection
                return GetPublicName(dbColumn);
            }
            else
            {
                // Part of a collection
                var sb = new StringBuilder();
                sb.Append("Get" + GetPublicName(coll));
                sb.Append(Formatter.OpenParameterString);
                sb.Append(Formatter.GetCast(coll.KeyType));
                sb.Append(item.Key);
                sb.Append(Formatter.CloseParameterString);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the code to use for the mutator for a <see cref="DbColumnInfo"/>.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the value mutator for.</param>
        /// <param name="valueName">Code to generate for the value to set.</param>
        /// <param name="columnSource">The name of the source collection if it is not in an instanced method. Can be null.</param>
        /// <returns>The code to use for the mutator for a <see cref="DbColumnInfo"/>.</returns>
        public string GetColumnValueMutator(DbColumnInfo dbColumn, string valueName, string columnSource = null)
        {
            ColumnCollectionItem item;
            var coll = GetCollectionForColumn(dbColumn, out item);

            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(columnSource) || columnSource.Trim().Length == 0)
                columnSource = "this";

            sb.Append(columnSource);
            sb.Append(".");

            if (coll == null)
            {
                // Not part of a collection
                sb.Append(GetPublicName(dbColumn));
                sb.Append(" = ");
                sb.Append(Formatter.GetCast(GetExternalType(dbColumn)));
                sb.Append(valueName);
                sb.Append(Formatter.EndOfLine);
            }
            else
            {
                // Part of a collection
                sb.Append("Set" + GetPublicName(coll));
                sb.Append(Formatter.OpenParameterString);
                sb.Append(Formatter.GetCast(coll.KeyType));
                sb.Append(item.Key);
                sb.Append(Formatter.ParameterSpacer);
                sb.Append(Formatter.GetCast(coll.ExternalType));
                sb.Append(valueName);
                sb.Append(Formatter.CloseParameterString);
                sb.Append(Formatter.EndOfLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the code string used for accessing a database <see cref="DbColumnInfo"/>'s value from a DataReader.
        /// </summary>
        /// <param name="column">The <see cref="DbColumnInfo"/> to get the value from.</param>
        /// <param name="ordinalFieldName">Name of the local field used to store the ordinal. The ordinal
        /// must already be assigned to this field.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>
        /// The code string used for accessing a database <see cref="DbColumnInfo"/>'s value.
        /// </returns>
        public string GetDataReaderAccessor(DbColumnInfo column, string ordinalFieldName, string variableName)
        {
            var callMethod = GetDataReaderReadMethodName(column.Type);

            // Find the method to use for reading the value
            var sb = new StringBuilder();

            // Cast
            sb.Append(Formatter.GetCast(GetExternalType(column)));

            // Accessor
            if (column.IsNullable)
            {
                sb.Append(Formatter.OpenParameterString);
                sb.Append(variableName);
                sb.Append(".IsDBNull");
                sb.Append(Formatter.OpenParameterString);
                sb.Append(ordinalFieldName);
                sb.Append(Formatter.CloseParameterString);
                sb.Append(" ? ");
                sb.Append(Formatter.GetCast(column.Type));
                sb.Append("null : ");
            }
            sb.Append(variableName);
            sb.Append(".");
            sb.Append(callMethod);
            sb.Append(Formatter.OpenParameterString);
            sb.Append(ordinalFieldName);
            sb.Append(Formatter.CloseParameterString);

            if (column.IsNullable)
                sb.Append(Formatter.CloseParameterString);

            return sb.ToString();
        }

        /// <summary>
        /// Gets the name of the method used by the DataReader to read the given Type.
        /// </summary>
        /// <param name="type">Type to read.</param>
        /// <param name="dataReaderReadMethods">The data reader read methods.</param>
        /// <returns>
        /// The name of the method used by the DataReader to read the given Type.
        /// </returns>
        public static string GetDataReaderReadMethodName(Type type, IDictionary<Type, string> dataReaderReadMethods)
        {
            if (type.IsNullable())
                type = type.GetNullableUnderlyingType();

            string callMethod;
            if (dataReaderReadMethods.TryGetValue(type, out callMethod))
                return callMethod;

            return "Get" + type.Name;
        }

        /// <summary>
        /// Gets the name of the method used by the DataReader to read the given Type.
        /// </summary>
        /// <param name="type">Type to read.</param>
        /// <returns>
        /// The name of the method used by the DataReader to read the given Type.
        /// </returns>
        public string GetDataReaderReadMethodName(Type type)
        {
            return GetDataReaderReadMethodName(type, _dataReaderReadMethods);
        }

        /// <summary>
        /// Gets a string for the Type used externally for a given column.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the external type for.</param>
        /// <returns>A string for the Type used externally for a given column.</returns>
        public string GetExternalType(DbColumnInfo dbColumn)
        {
            var ret = _externalTypes[dbColumn];
            if (dbColumn.IsNullable)
                ret = EnsureIsNullable(ret);

            return ret;
        }

        /// <summary>
        /// Gets a string for the Type used internally for a given column.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the internal type for.</param>
        /// <returns>A string for the Type used internally for a given column.</returns>
        public string GetInternalType(DbColumnInfo dbColumn)
        {
            var ret = Formatter.GetTypeString(dbColumn.Type);
            if (dbColumn.IsNullable)
                ret = EnsureIsNullable(ret);

            return ret;
        }

        /// <summary>
        /// Gets the parameter name for a <see cref="DbColumnInfo"/>.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the parameter name for.</param>
        /// <returns>The parameter name for the <see cref="DbColumnInfo"/>.</returns>
        public string GetParameterName(DbColumnInfo dbColumn)
        {
            return _parameterNames[dbColumn];
        }

        /// <summary>
        /// Gets the private name for a <see cref="DbColumnInfo"/>.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the private name for.</param>
        /// <returns>The private name for the <see cref="DbColumnInfo"/>.</returns>
        public string GetPrivateName(DbColumnInfo dbColumn)
        {
            return _privateNames[dbColumn];
        }

        /// <summary>
        /// Gets the private name for a <see cref="DbColumnInfo"/>.
        /// </summary>
        /// <param name="columnCollection">The <see cref="ColumnCollection"/> to get the name of.</param>
        /// <returns>The private name for the <see cref="DbColumnInfo"/>.</returns>
        public string GetPrivateName(ColumnCollection columnCollection)
        {
            return Formatter.GetFieldName(columnCollection.Name, MemberVisibilityLevel.Private, columnCollection.ExternalType);
        }

        /// <summary>
        /// Gets the public name for a <see cref="DbColumnInfo"/>.
        /// </summary>
        /// <param name="dbColumn">The <see cref="DbColumnInfo"/> to get the public name for.</param>
        /// <returns>The public name for the <see cref="DbColumnInfo"/>.</returns>
        public string GetPublicName(DbColumnInfo dbColumn)
        {
            return _publicNames[dbColumn];
        }

        /// <summary>
        /// Gets the public name for a <see cref="DbColumnInfo"/>.
        /// </summary>
        /// <param name="columnCollection">The <see cref="ColumnCollection"/> to get the name of.</param>
        /// <returns>The public name for the <see cref="DbColumnInfo"/>.</returns>
        public string GetPublicName(ColumnCollection columnCollection)
        {
            if (columnCollection.Name != "Stat")
            {
            }

            return Formatter.GetFieldName(columnCollection.Name, MemberVisibilityLevel.Public, columnCollection.ExternalType);
        }
    }
}