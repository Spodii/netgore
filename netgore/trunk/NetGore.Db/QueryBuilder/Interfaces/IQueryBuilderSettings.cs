using System;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for the settings of a <see cref="IQueryBuilder"/>.
    /// </summary>
    public interface IQueryBuilderSettings
    {
        /// <summary>
        /// Gets the <see cref="StringComparer"/> to use for comparing the column names for equality.
        /// Generally, this should be <see cref="StringComparer.Ordinal"/> for a DBMS with case-sensitive column names
        /// and <see cref="StringComparer.OrdinalIgnoreCase"/> for a DBMS with case-insensitive column names.
        /// </summary>
        StringComparer ColumnNameComparer { get; }

        /// <summary>
        /// Applies a column alias to a string.
        /// </summary>
        /// <param name="sql">The string containing the SQL that the <see cref="alias"/> will be added to.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>The aliased sql.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sql"/> is null or empty.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is an invalid column alias.</exception>
        string ApplyColumnAlias(string sql, string alias);

        /// <summary>
        /// Applies a table alias to a string.
        /// </summary>
        /// <param name="sql">The string containing the SQL that the <see cref="alias"/> will be added to.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>The aliased sql.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sql"/> is null or empty.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is an invalid table alias.</exception>
        string ApplyTableAlias(string sql, string alias);

        /// <summary>
        /// Escapes a column's name.
        /// </summary>
        /// <param name="columnName">The name of the column to escape.</param>
        /// <returns>The escaped <paramref name="columnName"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnName"/> is an invalid column name.</exception>
        string EscapeColumn(string columnName);

        /// <summary>
        /// Escapes a table's name.
        /// </summary>
        /// <param name="tableName">The name of the table to escape.</param>
        /// <returns>The escaped <paramref name="tableName"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        string EscapeTable(string tableName);

        /// <summary>
        /// Checks if a column alias is valid.
        /// </summary>
        /// <param name="columnAlias">The column alias. Can be null to signify an alias not being used.</param>
        /// <param name="throwOnInvalid">When true, an <see cref="InvalidQueryException"/> will be thrown when the
        /// <paramref name="columnAlias"/> is invalid.</param>
        /// <returns>True if the <paramref name="columnAlias"/> is valid; otherwise false. Cannot be false when
        /// <paramref name="throwOnInvalid"/> is true since an <see cref="InvalidQueryException"/> needs to be thrown instead.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnAlias"/> is an invalid column alias and
        /// <paramref name="throwOnInvalid"/> is true.</exception>
        bool IsValidColumnAlias(string columnAlias, bool throwOnInvalid = false);

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
        bool IsValidColumnName(string columnName, bool throwOnInvalid = false);

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
        bool IsValidParameterName(string parameterName, bool throwOnInvalid = false);

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
        bool IsValidTableAlias(string tableAlias, bool throwOnInvalid = false);

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
        bool IsValidTableName(string tableName, bool throwOnInvalid = false);

        /// <summary>
        /// Creates a query parameter identifier.
        /// </summary>
        /// <param name="parameterName">The name of the query parameter.</param>
        /// <returns>The query parameter identifier.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="parameterName"/> is an invalid parameter name.</exception>
        string Parameterize(string parameterName);
    }
}