using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// Provides methods that assist in making asserts on the database queries.
    /// </summary>
    public static class QueryAsserts
    {
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
                if (!primaryKeys.Contains(expectedKey, StringComparer.OrdinalIgnoreCase))
                    Debug.Fail(errmsg);
            }
        }

        /// <summary>
        /// Checks that the given set of primary keys contains the expected set of keys. The expected set of keys
        /// does not need to contain all primary keys.
        /// </summary>
        /// <param name="primaryKeys">The actual set of primary keys.</param>
        /// <param name="expectedKeys">The expected set of primary keys.</param>
        [Conditional("DEBUG")]
        public static void ContainsPrimaryKeys(IEnumerable<string> primaryKeys, params string[] expectedKeys)
        {
            const string errmsg = "Expected primary key `{0}` not found.";

            if (primaryKeys.Count() != expectedKeys.Length)
                Debug.Fail(errmsg);

            foreach (var expectedKey in expectedKeys)
            {
                if (!primaryKeys.Contains(expectedKey, StringComparer.OrdinalIgnoreCase))
                    Debug.Fail(string.Format(errmsg, expectedKey));
            }
        }
    }
}
