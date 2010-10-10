using System;
using System.Linq;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IQueryBuilderSettings"/> implementation for MySql.
    /// </summary>
    public class MySqlQueryBuilderSettings : IQueryBuilderSettings
    {
        static readonly MySqlQueryBuilderSettings _instance;

        /// <summary>
        /// Initializes the <see cref="MySqlQueryBuilderSettings"/> class.
        /// </summary>
        static MySqlQueryBuilderSettings()
        {
            _instance = new MySqlQueryBuilderSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlQueryBuilderSettings"/> class.
        /// </summary>
        MySqlQueryBuilderSettings()
        {
        }

        /// <summary>
        /// Gets the <see cref="MySqlQueryBuilderSettings"/> instance.
        /// </summary>
        public static MySqlQueryBuilderSettings Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Checks if a column name is valid.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="throwOnInvalid">When true, an <see cref="InvalidQueryException"/> will be thrown when the
        /// <paramref name="columnName"/> is invalid.</param>
        /// <returns>True if the <paramref name="columnName"/> is valid; otherwise false. Cannot be false when
        /// <paramref name="throwOnInvalid"/> is true since an <see cref="InvalidQueryException"/> needs to be thrown instead.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnName"/> is an invalid column name and
        /// <paramref name="throwOnInvalid"/> is true.</exception>
        public bool IsValidColumnName(string columnName, bool throwOnInvalid = false)
        {
            if (string.IsNullOrEmpty(columnName))
                throw new InvalidQueryException("The column name cannot be empty.");

            if (columnName.Contains(' '))
                throw new InvalidQueryException("The column name may not contain a space.");

            return true;
        }

        /// <summary>
        /// Checks if a parameter name is valid.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="throwOnInvalid">When true, an <see cref="InvalidQueryException"/> will be thrown when the
        /// <paramref name="parameterName"/> is invalid.</param>
        /// <returns>True if the <paramref name="parameterName"/> is valid; otherwise false. Cannot be false when
        /// <paramref name="throwOnInvalid"/> is true since an <see cref="InvalidQueryException"/> needs to be thrown instead.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="parameterName"/> is an invalid parameter name and
        /// <paramref name="throwOnInvalid"/> is true.</exception>
        public bool IsValidParameterName(string parameterName, bool throwOnInvalid)
        {
            if (string.IsNullOrEmpty(parameterName))
                throw new InvalidQueryException("The parameter name cannot be empty.");

            if (parameterName.Contains(' '))
                throw new InvalidQueryException("The parameter name may not contain a space.");

            return true;
        }

        /// <summary>
        /// Checks if a table name is valid.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="throwOnInvalid">When true, an <see cref="InvalidQueryException"/> will be thrown when the
        /// <paramref name="tableName"/> is invalid.</param>
        /// <returns>True if the <paramref name="tableName"/> is valid; otherwise false. Cannot be false when
        /// <paramref name="throwOnInvalid"/> is true since an <see cref="InvalidQueryException"/> needs to be thrown instead.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name and
        /// <paramref name="throwOnInvalid"/> is true.</exception>
        public bool IsValidTableName(string tableName, bool throwOnInvalid = false)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                if (throwOnInvalid)
                    throw new InvalidQueryException("The table name cannot be empty.");
                else
                    return false;
            }

            if (tableName.Contains(' '))
            {
                if (throwOnInvalid)
                    throw new InvalidQueryException("The table name may not contain a space.");
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a table alias is valid.
        /// </summary>
        /// <param name="tableAlias">The table alias. Can be null to signify an alias not being used.</param>
        /// <param name="throwOnInvalid">When true, an <see cref="InvalidQueryException"/> will be thrown when the
        /// <paramref name="tableAlias"/> is invalid.</param>
        /// <returns>True if the <paramref name="tableAlias"/> is valid; otherwise false. Cannot be false when
        /// <paramref name="throwOnInvalid"/> is true since an <see cref="InvalidQueryException"/> needs to be thrown instead.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableAlias"/> is an invalid table alias and
        /// <paramref name="throwOnInvalid"/> is true.</exception>
        public bool IsValidTableAlias(string tableAlias, bool throwOnInvalid)
        {
            if (tableAlias == null)
                return true;

            if (string.IsNullOrEmpty(tableAlias))
            {
                if (throwOnInvalid)
                    throw new InvalidQueryException("The table alias cannot be empty.");
                else
                    return false;
            }

            if (tableAlias.Contains(' '))
            {
                if (throwOnInvalid)
                    throw new InvalidQueryException("The table alias may not contain a space.");
                else
                    return false;
            }

            return true;
        }

        #region IQueryBuilderSettings Members

        /// <summary>
        /// Gets the <see cref="StringComparer"/> to use for comparing the column names for equality.
        /// Generally, this should be <see cref="StringComparer.Ordinal"/> for a DBMS with case-sensitive column names
        /// and <see cref="StringComparer.OrdinalIgnoreCase"/> for a DBMS with case-insensitive column names.
        /// </summary>
        public StringComparer ColumnNameComparer
        {
            get { return StringComparer.OrdinalIgnoreCase; }
        }

        /// <summary>
        /// Escapes a column's name.
        /// </summary>
        /// <param name="columnName">The name of the column to escape.</param>
        /// <returns>The escaped <paramref name="columnName"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnName"/> is an invalid column name.</exception>
        public string EscapeColumn(string columnName)
        {
            if (!columnName.Contains("."))
                return "`" + columnName + "`";
            else
                return columnName;
        }

        /// <summary>
        /// Escapes a table's name.
        /// </summary>
        /// <param name="tableName">The name of the table to escape.</param>
        /// <returns>The escaped <paramref name="tableName"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        public string EscapeTable(string tableName)
        {
            if (!tableName.Contains("."))
                return "`" + tableName + "`";
            else
                return tableName;
        }

        /// <summary>
        /// Creates a query parameter identifier.
        /// </summary>
        /// <param name="parameterName">The name of the query parameter.</param>
        /// <returns>The query parameter identifier.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="parameterName"/> is an invalid parameter name.</exception>
        public string Parameterize(string parameterName)
        {
            if (!parameterName.StartsWith("@"))
                return "@" + parameterName;
            else
            {
                // TODO: errmsg - already escaped
                return parameterName;
            }
        }

        #endregion
    }
}