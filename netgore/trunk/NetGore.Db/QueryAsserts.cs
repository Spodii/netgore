using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Provides methods that assist in making asserts on the database queries. This provides better run-time checking of the
    /// queries. It is recommended to use these <see cref="QueryAsserts"/> on all database query classes in their constructor,
    /// when possible. Otherwise, invalid queries might not throw any exceptions until you attempt to execute them, or the query
    /// may execute but not in the way you expected it to.
    /// All assertion methods are handled in debug mode only. When building the project in release mode, none of these methods will
    /// be executed, so there is no performance cost for the end product.
    /// </summary>
    public static class QueryAsserts
    {
        /// <summary>
        /// The StringComparer used for comparing column names.
        /// </summary>
        static readonly StringComparer _columnNameComparer = StringComparer.OrdinalIgnoreCase;

        /// <summary>
        /// Checks that the given set of <paramref name="columns"/> match the expected set of column names exactly, except for in order.
        /// </summary>
        /// <param name="columns">The actual set of columns.</param>
        /// <param name="expectedColumns">The expected set of columns.</param>
        [Conditional("DEBUG")]
        public static void AreColumns(IEnumerable<string> columns, params string[] expectedColumns)
        {
            const string errmsg = "The two sets of columns do not match perfectly.";

            if (columns.Count() != expectedColumns.Length)
                Debug.Fail(errmsg);

            foreach (var expectedKey in expectedColumns)
            {
                if (!columns.Contains(expectedKey, _columnNameComparer))
                    Debug.Fail(errmsg);
            }
        }

        /// <summary>
        /// Checks that the given set of primary keys match the expected set exactly, except for in order.
        /// </summary>
        /// <param name="primaryKeys">The actual set of primary keys.</param>
        /// <param name="expectedKeys">The expected set of primary keys.</param>
        [Conditional("DEBUG")]
        public static void ArePrimaryKeys(IEnumerable<string> primaryKeys, params string[] expectedKeys)
        {
            const string errmsg = "The two sets of primary keys do not match perfectly.";

            if (primaryKeys.Count() != expectedKeys.Length)
                Debug.Fail(errmsg);

            foreach (var expectedKey in expectedKeys)
            {
                if (!primaryKeys.Contains(expectedKey, _columnNameComparer))
                    Debug.Fail(errmsg);
            }
        }

        /// <summary>
        /// Checks if the given set of <paramref name="columns"/> contains the expected set of
        /// <paramref name="expectedColumns"/>. The expected set of <paramref name="columns"/> does not
        /// need to contain all of the actual <paramref name="columns"/>.
        /// </summary>
        /// <param name="columns">The actual set of database columns.</param>
        /// <param name="expectedColumns">The expected set of columns to exist.</param>
        [Conditional("DEBUG")]
        public static void ContainsColumns(IEnumerable<string> columns, params string[] expectedColumns)
        {
            const string errmsg = "Expected column `{0}` not found.";

            foreach (var expectedColumn in expectedColumns)
            {
                if (!columns.Contains(expectedColumn, _columnNameComparer))
                    Debug.Fail(string.Format(errmsg, expectedColumn));
            }
        }

        /// <summary>
        /// Checks that the given set of primary keys contains the expected set of keys. The expected set of keys
        /// does not need to contain all primary keys.
        /// </summary>
        /// <param name="primaryKeys">The actual set of primary keys.</param>
        /// <param name="expectedKeys">The expected set of primary keys to exist.</param>
        [Conditional("DEBUG")]
        public static void ContainsPrimaryKeys(IEnumerable<string> primaryKeys, params string[] expectedKeys)
        {
            const string errmsg = "Expected primary key `{0}` not found.";

            foreach (var expectedKey in expectedKeys)
            {
                if (!primaryKeys.Contains(expectedKey, _columnNameComparer))
                    Debug.Fail(string.Format(errmsg, expectedKey));
            }
        }
    }
}