using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for an SQL INSERT ... ON DUPLICATE KEY UPDATE query.
    /// </summary>
    public interface IInsertODKUQuery : IColumnValueCollectionBuilder<IInsertODKUQuery>
    {
        /// <summary>
        /// Copies the column names and values from the insert statement into the update statement.
        /// </summary>
        /// <example>
        /// For the following INSERT query:
        ///     INSERT INTO myTable (a,b,c) VALUES (@a,@b,1)
        ///     
        /// You will get the ON DUPLICATE KEY UPDATE query:
        ///     ... ON DUPLICATE KEY UPDATE SET a=@a,b=@b,c=1
        /// </example>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        IInsertODKUQuery AddFromInsert();

        /// <summary>
        /// Copies the column names and values from the insert statement into the update statement.
        /// </summary>
        /// <param name="except">The name of the column to exclude from the update statement.</param>
        /// <example>
        /// For the following INSERT query:
        ///     INSERT INTO myTable (a,b,c) VALUES (@a,@b,1)
        ///     
        /// You will get the ON DUPLICATE KEY UPDATE query:
        ///     ... ON DUPLICATE KEY UPDATE SET a=@a,b=@b,c=1
        /// </example>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="except"/> is not a valid column name.</exception>
        IInsertODKUQuery AddFromInsert(string except);

        /// <summary>
        /// Copies the column names and values from the insert statement into the update statement.
        /// </summary>
        /// <param name="except">The names of the columns to exclude from the update statement.</param>
        /// <example>
        /// For the following INSERT query:
        ///     INSERT INTO myTable (a,b,c) VALUES (@a,@b,1)
        ///     
        /// You will get the ON DUPLICATE KEY UPDATE query:
        ///     ... ON DUPLICATE KEY UPDATE SET a=@a,b=@b,c=1
        /// </example>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="except"/> contains one or more invalid column names.</exception>
        IInsertODKUQuery AddFromInsert(IEnumerable<string> except);

        /// <summary>
        /// Copies the column names and values from the insert statement into the update statement.
        /// </summary>
        /// <param name="except">The names of the columns to exclude from the update statement.</param>
        /// <example>
        /// For the following INSERT query:
        ///     INSERT INTO myTable (a,b,c) VALUES (@a,@b,1)
        ///     
        /// You will get the ON DUPLICATE KEY UPDATE query:
        ///     ... ON DUPLICATE KEY UPDATE SET a=@a,b=@b,c=1
        /// </example>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="except"/> contains one or more invalid column names.</exception>
        IInsertODKUQuery AddFromInsert(params string[] except);
    }
}