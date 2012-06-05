using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Helper methods for creating IDs in a database.
    /// </summary>
    public static class IDCreatorHelper
    {
        /// <summary>
        /// Gets the next free ID in a database column.
        /// </summary>
        /// <param name="pool">The pool.</param>
        /// <param name="table">The database table.</param>
        /// <param name="column">The table column.</param>
        /// <returns>The next free ID.</returns>
        public static int GetNextFreeID(DbConnectionPool pool, string table, string column)
        {
            using (var creator = new IntIDCreator(pool, table, column, 2))
            {
                return creator.GetNext();
            }
        }

        /// <summary>
        /// A <see cref="IDCreatorBase{T}"/> for an int.
        /// </summary>
        class IntIDCreator : IDCreatorBase<int>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="IntIDCreator"/> class.
            /// </summary>
            /// <param name="connectionPool">The connection pool.</param>
            /// <param name="table">The table.</param>
            /// <param name="column">The column.</param>
            /// <param name="stackSize">Size of the stack.</param>
            public IntIDCreator(DbConnectionPool connectionPool, string table, string column, int stackSize)
                : base(connectionPool, table, column, stackSize)
            {
            }

            /// <summary>
            /// When overridden in the derived class, converts the given int to type int.
            /// </summary>
            /// <param name="value">The value to convert to type int.</param>
            /// <returns>The <paramref name="value"/> as type int.</returns>
            protected override int FromInt(int value)
            {
                return value;
            }

            /// <summary>
            /// When overridden in the derived class, converts the given value of type int to an int.
            /// </summary>
            /// <param name="value">The value to convert to an int.</param>
            /// <returns>The int value of the <paramref name="value"/>.</returns>
            protected override int ToInt(int value)
            {
                return value;
            }
        }
    }
}